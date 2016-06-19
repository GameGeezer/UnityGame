using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Octree<T> {

    public Vector3 leafDimensions { get; private set; }

    private OctreeBodyNode<T> root;

    public SafePool<OctreeEntry<T>> entryPool = new SafePool<OctreeEntry<T>>();
    public SafePool<OctreeBodyNode<T>> bodyNodePool = new SafePool<OctreeBodyNode<T>>();
    public SafePool<OctreeLeafNode<T>> leafNodePool = new SafePool<OctreeLeafNode<T>>();
    public SafePool<Vector3> vector3Pool = new SafePool<Vector3>();

    public Dictionary<int, float> cellsAccrossAtLevel = new Dictionary<int, float>();
    public Dictionary<int, float> halfCellsAccrossAtLevel = new Dictionary<int, float>();
    public Dictionary<Vector3i, T> brickDictionary = new Dictionary<Vector3i, T>();

    public Octree(Vector3 leafDimensions, Vector3i startPosition)
    {
        this.leafDimensions = leafDimensions;
        // Maintain dimensions in the Octree to avoid dublicating throught the leaves
        CreateDimensionsAtLevel(0);
        CreateDimensionsAtLevel(1);
        // Create the root node
        OctreeBodyNode<T> rootNode = bodyNodePool.Catch();
        // Initialize the root node
        root = rootNode.ReInitialize(this, startPosition, 1);    
    }

    public void RayCastFind(Ray ray, PriorityQueue<float, OctreeEntry<T>> found)
    {
        root.RaycastFind(ray, found);
    }

    public T GetAt(Vector3i point)
    {
        if (brickDictionary.ContainsKey(point))
        {
            // The cell lies within the tree's bounds
            return brickDictionary[point];
        }
        else
        {
            // The tree does not contain the cell
            return default(T);
        }
    }

    public void SetAt(Vector3i point, T value)
    {
        // The cell is within the current octree
        if (root.Contains(point))
        {
            if(brickDictionary.ContainsKey(point))
            {
                brickDictionary[point] = value;
            }
            else
            {
                brickDictionary.Add(point, value);
            }

            root.SetAt(point, value);
        }
        else
        {
            // Grow the octree towards the cell
            GrowTowards(point);
            // Attempt to set again
            SetAt(point, value);
        }
    }

    public void RemoveAt(Vector3i point)
    {
        if (root.Contains(point))
        {
            if (brickDictionary.ContainsKey(point))
            {
                brickDictionary.Remove(point);
            }

            root.RemoveAt(point);
        }
    }

    public void DrawWireFrame()
    {
        root.Draw();
    }

    public void CreateDimensionsAtLevel(int level)
    {
        float cellsAccross = (int)Mathf.Pow(2, level + 1);

        cellsAccrossAtLevel[level] = cellsAccross;

        halfCellsAccrossAtLevel[level] = cellsAccross / 2.0f;
    }

    private void GrowTowards(Vector3i point)
    {
        // Add new entries to the dimension dictionaries
        CreateDimensionsAtLevel(root.Level + 1);
        // Find the direction of the point relative to the root
        int xDirection, yDirection, zDirection;
        root.DirectionTowardsPoint(point, out xDirection, out yDirection, out zDirection);
        // Find the weights which will be used to create the OcteeChild index
        int xIndexMod = xDirection * OctreeConstants.X_WEIGHT;
        int yIndexMod = yDirection * OctreeConstants.Y_WEIGHT;
        int zIndexMod = zDirection * OctreeConstants.Z_WEIGHT;
        // The index inwhich the current root node will be placed
        OctreeChild moveRootTo = (OctreeChild)(xIndexMod + yIndexMod + zIndexMod);
        // Flip the direction (1 becomes 0, and 0 becomes 1)
        xDirection = Convert.ToInt32(xDirection == 0);
        yDirection = Convert.ToInt32(yDirection == 0);
        zDirection = Convert.ToInt32(zDirection == 0);

        int cellsAccross = (int)cellsAccrossAtLevel[root.Level];

        int rootMinX = ((xDirection) * cellsAccross); //+ (int)root.bounds.min.x;
        int rootMinY = ((yDirection) * cellsAccross); //+ (int)root.bounds.min.y;
        int rootMinZ = ((zDirection) * cellsAccross); //+ (int)root.bounds.min.z;

        OctreeBodyNode<T> newRootNode = bodyNodePool.Catch();

        newRootNode.ReInitialize(this, new Vector3i(rootMinX, rootMinY, rootMinZ), root.Level + 1);

        newRootNode.PlaceChild(moveRootTo, root);

        root = newRootNode;
    }
}