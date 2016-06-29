using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Octree<T> {

    public Vector3 leafDimensions { get; private set; }

    public Pool<OctreeEntry<T>> entryPool { get; private set; }

    public Pool<OctreeBodyNode<T>> bodyNodePool { get; private set; }

    public Pool<OctreeLeafNode<T>> leafNodePool { get; private set; }

    public Dictionary<int, float> cellsAccrossAtLevel { get; private set; }

    public Dictionary<int, float> halfCellsAccrossAtLevel { get; private set; }

    private Dictionary<int, OctreeEntry<T>> quickieDictionary;

    private OctreeBodyNode<T> root;

    public Octree(Vector3 leafDimensions, Vector3i startPosition)
    {
        this.leafDimensions = leafDimensions;

        entryPool = new Pool<OctreeEntry<T>>();

        bodyNodePool = new Pool<OctreeBodyNode<T>>();

        leafNodePool = new Pool<OctreeLeafNode<T>>();

        cellsAccrossAtLevel = new Dictionary<int, float>();

        halfCellsAccrossAtLevel = new Dictionary<int, float>();

        quickieDictionary = new Dictionary<int, OctreeEntry<T>>();
        
        // Maintain dimensions in the Octree to avoid dublicating throughout the leaves
        CreateDimensionsAtLevel(0);

        CreateDimensionsAtLevel(1);
        
        root = bodyNodePool.Catch();
        
        root.Initialize(this, startPosition.x, startPosition.y, startPosition.z, 1);    
    }

    public void RayCastFind(Ray ray, PriorityQueue<OctreeEntry<T>, float> found)
    {
        root.RaycastFind(ray, found);
    }

    public OctreeEntry<T> GetAt(int x, int y, int z)
    {
        int hashIndex = Vector3i.Hash(x, y, z);

        if (quickieDictionary.ContainsKey(hashIndex))
        {
            // The cell lies within the tree's bounds
            return quickieDictionary[hashIndex];
        }

        // The tree does not contain the cell
        return default(OctreeEntry<T>);
    }

    public OctreeEntry<T> SetAt(int x, int y, int z, T value)
    {
        if (root.Contains(x, y, z))
        {
            OctreeEntry<T> entry = root.SetAt(x, y, z, value);

            quickieDictionary[Vector3i.Hash(x, y, z)] = entry;

            return entry;
        }

        // Grow the octree towards the cell
        GrowTowards(x, y, z);

        // Attempt to set again
        return SetAt(x, y, z, value);
    }

    public void DrawGizmos()
    {
        root.DrawGizmos();
    }

    public void CreateDimensionsAtLevel(int level)
    {
        float cellsAccross = (int)Mathf.Pow(2, level + 1);

        cellsAccrossAtLevel[level] = cellsAccross;

        halfCellsAccrossAtLevel[level] = cellsAccross / 2.0f;
    }

    private void GrowTowards(int x, int y, int z)
    {
        // Add new entries to the dimension dictionaries
        CreateDimensionsAtLevel(root.level + 1);
        // Find the direction of the point relative to the root
        int xDirection, yDirection, zDirection;
        root.DirectionTowardsPoint(x, y, z, out xDirection, out yDirection, out zDirection);
        // Find the weights which will be used to create the OcteeChild index
        int xIndexMod = xDirection * OctreeConstants.X_WEIGHT;
        int yIndexMod = yDirection * OctreeConstants.Y_WEIGHT;
        int zIndexMod = zDirection * OctreeConstants.Z_WEIGHT;
        // The index inwhich the current root node will be placed
        int moveRootTo = xIndexMod + yIndexMod + zIndexMod;
        // Flip the direction (1 becomes 0, and 0 becomes 1)

        int cellsAccross = (int)halfCellsAccrossAtLevel[root.level + 1];

        int rootMinX = (int)root.cellsCovered.low.x - ((xDirection) * cellsAccross); //+ (int)root.bounds.min.x;
        int rootMinY = (int)root.cellsCovered.low.y - ((yDirection) * cellsAccross); //+ (int)root.bounds.min.y;
        int rootMinZ = (int)root.cellsCovered.low.z - ((zDirection) * cellsAccross); //+ (int)root.bounds.min.z;

        OctreeBodyNode<T> newRootNode = bodyNodePool.Catch();

        newRootNode.Initialize(this, rootMinX, rootMinY, rootMinZ, root.level + 1); // CANT EXPAND INTO NEGATIVES BECASUE MIN IS ALWAYS 000

        newRootNode.PlaceChild(moveRootTo, root);

        root = newRootNode;
    }
}