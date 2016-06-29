using System;
using UnityEngine;

public class OctreeBodyNode<T> : OctreeNode<T>
{
    private OctreeNode<T>[] children = new OctreeNode<T>[8];

    public OctreeBodyNode()
    {
        
    }

    public OctreeBodyNode<T> Initialize(Octree<T> treeBase, int xMinimum, int yMinimum, int zMinimum, int level)
    {
        BaseInitialize(treeBase, xMinimum, yMinimum, zMinimum, level);

        for (int i = 0; i < OctreeConstants.NUMBER_OF_CHILDREN; ++i)
        {
            children[i] = null;
        }

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

    public override OctreeEntry<T> SetAt(int x, int y, int z, T value)
    {
        int index = ChildRelativeTo(x, y, z);

        if (children[index] != null)
        {
            // The cell is in the child at index
            return children[index].SetAt(x, y, z, value);
        }

        // Find the new child min
        int xMinimum, yMinimum, zMinimum;
        MinOfChildIndex(index, out xMinimum, out yMinimum, out zMinimum);

        if (level == OctreeConstants.BODY_NODE_BASE_LEVEL)
        {
            // Create a leaf node if the level is below BODY_NODE_BASE_LEVEL
            OctreeLeafNode<T> fish = treeBase.leafNodePool.Catch();
            // initialize the leaf node
            fish.Initialize(treeBase, xMinimum, yMinimum, zMinimum);
            // Set the child at index to the new node
            SetChild(index, fish);
        }
        else // Create a body node
        {
            //Requesta new body node
            OctreeBodyNode<T> fish = treeBase.bodyNodePool.Catch();
            // initialize the body node
            fish.Initialize(treeBase, xMinimum, yMinimum, zMinimum, level - 1);
            // Set the child at index to the new node
            SetChild(index, fish);
        }

        return children[index].SetAt(x, y, z, value);
    }

    public override void DrawGizmos()
    {
        for (int i = 0; i < 8; ++i)
        {
            if (children[i] == null)
            {
                continue;
            }

            children[i].DrawGizmos();
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(worldBounds.center, worldBounds.extents * 2);
    }

    public void PlaceChild(int index, OctreeNode<T> node)
    {
        if(level == 1 && node.GetType() == typeof(OctreeLeafNode<T>))
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

    protected void SetChild(int index, OctreeNode<T> node)
    {
        if (children[index] == null)
        {
            ++childCount;
        }

        children[index] = node;
    }
}

