using System;
using UnityEngine;

public class OctreeBodyNode<T> : OctreeNode<T>
{
    private OctreeNode<T>[] children = new OctreeNode<T>[8];

    public OctreeBodyNode()
    {
        
    }

    public OctreeBodyNode<T> ReInitialize(Octree<T> treeBase, Vector3i center, int level)
    {
        Initialize(treeBase, center, level);

        return this;
    }

    public override T GetAt(Vector3 point)
    {
        int index = (int)ChildRelativeTo(point);

        if(children[index] == null)
        {
            return default(T);
        }
        
        return children[index].GetAt(point);
    }

    public override void RaycastFind(Ray ray, PriorityQueue<T> found)
    {
        if(!bounds.IntersectRay(ray))
        {
            return;
        }

        for (int i = 0; i < 8; ++i)
        {
            if(children[i] == null)
            {
                continue;
            }

            children[i].RaycastFind(ray, found);
        }
    }

    public override void SetAt(Vector3 point, T value)
    {
        int index = (int)ChildRelativeTo(point);

        if (children[index] == null)
        {
            if(Level == 1)
            {
                Vector3i nodeCenter = CenterOfChildIndex(index);

                OctreeLeafNode<T> fish = treeBase.leafNodePool.Catch();

                fish.ReInitialize(treeBase, nodeCenter);

                SetChild((OctreeChild)index, fish);
            }
            else
            {
                Vector3i nodeCenter = CenterOfChildIndex(index);

                OctreeBodyNode<T> fish = treeBase.bodyNodePool.Catch();

                fish.ReInitialize(treeBase, nodeCenter, Level - 1);

                SetChild((OctreeChild)index, fish);
            }
        }

        children[index].SetAt(point, value);
    }

    public override bool RemoveAt(Vector3 point)
    {
        int index = (int)ChildRelativeTo(point);

        if (children[index] == null)
        {
            return HasChildren();
        }

        bool childClear = children[index].RemoveAt(point);

        if(childClear)
        {
            RemoveChild((OctreeChild)index);
        }

        return HasChildren();
    }

    public void PlaceChild(OctreeChild index, OctreeNode<T> node)
    {
        if(Level == 1 && node.GetType() == typeof(OctreeLeafNode<T>))
        {
            SetChild(index, node);
        }
        else if(node.GetType() == typeof(OctreeBodyNode<T>))
        {
            SetChild(index, node);
        }
        else
        {
            //TODO throw exception
        }
    }

    protected void SetChild(OctreeChild index, OctreeNode<T> node)
    {
        if (children[(int)index] == null)
        {
            ++childCount;
        }

        children[(int)index] = node;
    }

    protected void RemoveChild(OctreeChild index)
    {
        if (children[(int)index] != null)
        {
            --childCount;
        }

        children[(int)index] = null;
    }
}
