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

    public override void RaycastFind(Ray ray, PriorityQueue<float, OctreeEntry<T>> found)
    {
        if (!worldBounds.IntersectRay(ray))
        {
            return;
        }

        for (int i = 0; i < 8; ++i)
        {
            float distance;
            if (children[i] == null || !children[i].bounds.IntersectRay(ray, out distance))
            {
                continue;
            }

            found.Enqueue(children[i], BrickConstants.LARGE_FLOAT - distance); // TODO propper priority
        }
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

    public override void Draw()
    {
   
        for (int i = 0; i < 8; ++i)
        {
            if (children[i] == null)
            {
                continue;
            }

            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(children[i].bounds.center, children[i].bounds.extents * 2);
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(worldBounds.center, worldBounds.extents * 2);
    }

    protected void SetChild(OctreeChild index, T node)
    {
        if(node == null)
        {
            RemoveChild(index);

            return;
        }

        if (children[(int)index] == null)
        {
            Vector3i min = MinOfChildIndex((int)index);

            OctreeEntry<T> fish = treeBase.entryPool.Catch();
            float halfX = treeBase.leafDimensions.x - 1;
            float halfY = treeBase.leafDimensions.y - 1;
            float halfZ = treeBase.leafDimensions.z - 1;

            float minX = min.x * treeBase.leafDimensions.x;
            float minY = min.y * treeBase.leafDimensions.y;
            float minZ = min.z * treeBase.leafDimensions.z;

            fish.ReInitialize(node, new Vector3(minX, minY, minZ), new Vector3(minX + halfX, minY + halfY, minZ + halfZ));
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
