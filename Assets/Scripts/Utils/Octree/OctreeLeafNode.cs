using UnityEngine;
using System.Collections;

public class OctreeLeafNode<T> : OctreeNode<T>
{
    private OctreeEntry<T>[] children = new OctreeEntry<T>[8];

    public OctreeLeafNode()
    {

    }

    public OctreeLeafNode<T> ReInitialize(Octree<T> treeBase, Vector3i center)
    {
        Initialize(treeBase, center, 0);

        return this;
    }

    public override OctreeEntry<T> GetAt(Vector3i point)
    {
        OctreeChild index = ChildRelativeTo(point);

        return children[(int)index];
    }

    public override void SetAt(Vector3i point, T value)
    {
        OctreeChild index = ChildRelativeTo(point);

        SetChild(index, value);
    }

    public override bool RemoveAt(Vector3i point)
    {
        OctreeChild index = ChildRelativeTo(point);

        if (children[(int)index] == null)
        {
            return HasChildren();
        }

        RemoveChild(index);

        return HasChildren();
    }

    public override void RaycastFind(Ray ray, PriorityQueue<float, OctreeEntry<T>> found)
    {
        if (!worldBounds.IntersectRay(ray))
        {
            return;
        }

        for (int i = 0; i < 8; ++i)
        {
            if(children[i] == null || !children[i].bounds.IntersectRay(ray))
            {
                continue;
            }

            found.Enqueue(children[i], 1); // TODO propper priority
        }
    }

    protected void SetChild(OctreeChild index, T node)
    {
        if(node == null)
        {
            RemoveChild(index);
        }

        if (children[(int)index] == null)
        {
            Vector3i center = CenterOfChildIndex((int)index);

            OctreeEntry<T> fish = treeBase.entryPool.Catch();
            float halfX = treeBase.leafDimensions.x / 2;
            float halfY = treeBase.leafDimensions.y / 2;
            float halfZ = treeBase.leafDimensions.z / 2;
            float centerX = center.x * treeBase.leafDimensions.x;
            float centerY = center.y * treeBase.leafDimensions.y;
            float centerZ = center.z * treeBase.leafDimensions.z;
            fish.ReInitialize(node, new Vector3(centerX - halfX, centerY - halfY, centerZ - halfZ), new Vector3(centerX + halfX, centerY + halfY, centerZ + halfZ));
            children[(int)index] = fish;

            ++childCount;
        }

        children[(int)index].entry = node;
    }

    protected void RemoveChild(OctreeChild index)
    {
        if (children[(int)index] != null && children[(int)index].entry != null)
        {
            --childCount;
        }

        children[(int)index].Clean();

        treeBase.entryPool.Release(children[(int)index]);

        children[(int)index] = null;
    }
}
