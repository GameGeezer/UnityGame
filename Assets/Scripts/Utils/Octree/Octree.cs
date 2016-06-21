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

    private Dictionary<Vector3i, T> quickieDictionary = new Dictionary<Vector3i, T>();

    private OctreeBodyNode<T> root;

    public Octree(Vector3 leafDimensions, Vector3i startPosition)
    {
        this.leafDimensions = leafDimensions;

        entryPool = new Pool<OctreeEntry<T>>();
        bodyNodePool = new Pool<OctreeBodyNode<T>>();
        leafNodePool = new Pool<OctreeLeafNode<T>>();

        cellsAccrossAtLevel = new Dictionary<int, float>();
        halfCellsAccrossAtLevel = new Dictionary<int, float>();
        
        // Maintain dimensions in the Octree to avoid dublicating throught the leaves
        CreateDimensionsAtLevel(0);
        CreateDimensionsAtLevel(1);
        // Create the root node
        OctreeBodyNode<T> rootNode = bodyNodePool.Catch();
        // Initialize the root node
        root = rootNode.ReInitialize(this, startPosition, 1);    
    }

    public void RayCastFind(Ray ray, PriorityQueue<OctreeEntry<T>, float> found)
    {
        root.RaycastFind(ray, found);
    }

    public T GetAt(Vector3i point)
    {
        if (quickieDictionary.ContainsKey(point))
        {
            // The cell lies within the tree's bounds
            return quickieDictionary[point];
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
            if(quickieDictionary.ContainsKey(point))
            {
                quickieDictionary[point] = value;
            }
            else
            {
                quickieDictionary.Add(point, value);
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

    public T RemoveAt(Vector3i point)
    {
        T removedEntry = default(T);
        // Is the point within the tree
        if (root.Contains(point))
        {
            // Remove the point from the dictionary
            quickieDictionary.Remove(point);

            root.RemoveAt(point, out removedEntry);
        }

        return removedEntry;
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

        int cellsAccross = (int)halfCellsAccrossAtLevel[root.Level + 1];

        int rootMinX = ((xDirection) * cellsAccross); //+ (int)root.bounds.min.x;
        int rootMinY = ((yDirection) * cellsAccross); //+ (int)root.bounds.min.y;
        int rootMinZ = ((zDirection) * cellsAccross); //+ (int)root.bounds.min.z;

        OctreeBodyNode<T> newRootNode = bodyNodePool.Catch();

        newRootNode.ReInitialize(this, new Vector3i(0, 0, 0), root.Level + 1); // CANT EXPAND INTO NEGATIVES BECASUE MIN IS ALWAYS 000

        newRootNode.PlaceChild(moveRootTo, root);

        root = newRootNode;
    }
}