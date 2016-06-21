using System;
using UnityEngine;

public class OctreeBodyNode<T> : OctreeNode<T>
{
    private OctreeNode<T>[] children = new OctreeNode<T>[8];

    public OctreeBodyNode()
    {
        
    }

    public OctreeBodyNode<T> ReInitialize(Octree<T> treeBase, Vector3i min, int level)
    {
        Initialize(treeBase, min, level);

        //TODO reset chilren @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@

        return this;
    }

    public override void RaycastFind(Ray ray, PriorityQueue<OctreeEntry<T>, float> found)
    {
        if (!worldBounds.IntersectRay(ray))
        {
            return;
        }

        for (int i = 0; i < 8; ++i)
        {
            if (children[i] == null)
            {
                continue;
            }

            children[i].RaycastFind(ray, found);
        }
    }

    public override OctreeEntry<T> GetAt(Vector3i point)
    {
        int index = (int)ChildRelativeTo(point);
        // The child is null meaning that the tree does not contain the cell
        if(children[index] == null)
        {
            return default(OctreeEntry<T>);
        }
        // Search the child for the cell
        return children[index].GetAt(point);
    }

    public override void SetAt(Vector3i point, T value)
    {
        int index = (int)ChildRelativeTo(point);
        // If the child index does not have a node
        if (children[index] == null)
        {
            // Find the new child center
            Vector3i nodeMin = MinOfChildIndex(index);

            // Create a leaf node if the level is below BODY_NODE_BASE_LEVEL
            if (Level == OctreeConstants.BODY_NODE_BASE_LEVEL)
            {
                // Request a new leaf node
                OctreeLeafNode<T> fish = treeBase.leafNodePool.Catch();
                // initialize the leaf node
                fish.ReInitialize(treeBase, nodeMin);
                // Set the child at index to the new node
                SetChild((OctreeChild)index, fish);
            }
            else // Create a body node
            {
                //Requesta new body node
                OctreeBodyNode<T> fish = treeBase.bodyNodePool.Catch();
                // initialize the body node
                fish.ReInitialize(treeBase, nodeMin, Level - 1);
                // Set the child at index to the new node
                SetChild((OctreeChild)index, fish);
            }
        }
        // The cell is in the child at index
        children[index].SetAt(point, value);
    }

    public override bool RemoveAt(Vector3i point, out T entry)
    {
        int index = (int)ChildRelativeTo(point);

        if (children[index] == null)
        {
            entry = default(T);

            return HasChildren();
        }

        if (children[index].RemoveAt(point, out entry))
        {
            RemoveChild((OctreeChild)index);
        }

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

            children[i].Draw();
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(worldBounds.center, worldBounds.extents * 2);
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
        OctreeNode<T> child = children[(int)index];

        if (child != null)
        {
            // Decrement the child counter
            --childCount;
            // Release the child
            if(child is OctreeBodyNode<T>)
            {
                treeBase.bodyNodePool.Release((OctreeBodyNode<T>)child);
            }
            else if(child is OctreeLeafNode<T>)
            {
                treeBase.leafNodePool.Release((OctreeLeafNode<T>)child);
            }
        }

        children[(int)index] = null;
    }
}

