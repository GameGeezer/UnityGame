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

    public override T GetAt(Vector3 point)
    {
        OctreeChild index = ChildRelativeTo(point);

        if(children[(int)index] != null)
        {
            return children[(int)index].entry;
        }

        return default(T);
    }

    public override void RaycastFind(Ray ray, PriorityQueue<T> found)
    {
        if (!bounds.IntersectRay(ray))
        {
            return;
        }

        for (int i = 0; i < 8; ++i)
        {
            if(!children[i].bounds.IntersectRay(ray))
            {
                continue;
            }

            found.Enqueue(children[i].entry, 1); // TODO propper priority
        }
    }

    public override bool RemoveAt(Vector3 point)
    {
        OctreeChild index = ChildRelativeTo(point);

        if (children[(int)index] == null)
        {
            return HasChildren();
        }

        RemoveChild(index);

        return HasChildren();
    }

    public override void SetAt(Vector3 point, T value)
    {
        OctreeChild index = ChildRelativeTo(point);

        SetChild(index, value);
    }

    protected void SetChild(OctreeChild index, T node)
    {
        if(node == null)
        {
            return;
        }

        if (children[(int)index] == null)
        {
            Vector3i center = CenterOfChildIndex((int)index);

            OctreeEntry<T> fish = treeBase.entryPool.Catch();
            fish.ReInitialize(node, new Vector3(center.x - 0.5f, center.y - 0.5f, center.z - 0.5f), new Vector3(center.x + 0.5f, center.y + 0.5f, center.z + 0.5f));
            children[(int)index] = fish;
        }

        if(children[(int)index].entry == null)
        {
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

        children[(int)index].entry = default(T);
    }
}
