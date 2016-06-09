﻿using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Octree<T> {

    private OctreeBodyNode<T> root;
    public Vector3 leafDimensions;

    public Pool<OctreeEntry<T>> entryPool = new Pool<OctreeEntry<T>>();
    public Pool<OctreeBodyNode<T>> bodyNodePool = new Pool<OctreeBodyNode<T>>();
    public Pool<OctreeLeafNode<T>> leafNodePool = new Pool<OctreeLeafNode<T>>();
    public Pool<Vector3> vector3Pool = new Pool<Vector3>();

    public Dictionary<int, float> cellsAccrossAtLevel = new Dictionary<int, float>();
    public Dictionary<int, float> halfCellsAccrossAtLevel = new Dictionary<int, float>();

    public Octree(Vector3 leafDimensions, Vector3i startPosition)
    {
        this.leafDimensions = leafDimensions;
        CreateDimensionsAtLevel(0);
        CreateDimensionsAtLevel(1);
        // Create the root node
        OctreeBodyNode<T> rootNode = bodyNodePool.Catch();
        // Initialize the root node
        root = rootNode.ReInitialize(this, startPosition, 1);    
    }

    public OctreeEntry<T> GetAt(Vector3i point)
    {
        if (root.Contains(point))
        {
            // The cell lies within the tree's bounds
            return root.GetAt(point);
        }
        else
        {
            // The tree does not contain the cell
            return default(OctreeEntry<T>);
        }
    }

    public void SetAt(Vector3i point, T value)
    {
        // The cell is within the current octree
        if (root.Contains(point))
        {
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
            root.RemoveAt(point);
        }
    }

    public void RayCastFind(Ray ray, PriorityQueue<OctreeEntry<T>> found)
    {
        root.RaycastFind(ray, found);
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
        int xDirection = Convert.ToInt32((point.x - root.GetCenterX()) < 0);
        int yDirection = Convert.ToInt32((point.y - root.GetCenterY()) < 0);
        int zDirection = Convert.ToInt32((point.z - root.GetCenterZ()) < 0);
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

        int halfCellsAccross = (int)halfCellsAccrossAtLevel[root.Level];

        int rootCenterX = ((xDirection * 2) - 1) * halfCellsAccross;
        int rootCenterY = ((yDirection * 2) - 1) * halfCellsAccross;
        int rootCenterZ = ((zDirection * 2) - 1) * halfCellsAccross;

        OctreeBodyNode<T> newRootNode = bodyNodePool.Catch();

        newRootNode.ReInitialize(this, new Vector3i(rootCenterX, rootCenterY, rootCenterZ), root.Level + 1);

        newRootNode.PlaceChild(moveRootTo, root);

        root = newRootNode;
    }
}
