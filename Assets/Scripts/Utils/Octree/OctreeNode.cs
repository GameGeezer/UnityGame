using System;
using UnityEngine;

public abstract class OctreeNode<T> {

    public int Level { get; private set; }

    protected Bounds bounds, worldBounds;

    protected int childCount { get; set; }

    protected Octree<T> treeBase;

    public OctreeNode()
    {
        
    }

    protected void Initialize(Octree<T> treeBase, Vector3i center, int level)
    {
        Level = level;
        this.treeBase = treeBase;
        childCount = 0;
        float cellsAccross = treeBase.cellsAccrossAtLevel[level];

        Vector3 boundsCenter = new Vector3(center.x, center.y, center.z);
        Vector3 boundsDimensions = new Vector3(cellsAccross, cellsAccross, cellsAccross);
        bounds = new Bounds(boundsCenter, boundsDimensions);

        Vector3 worldBoundsCenter = new Vector3(center.x * treeBase.leafDimensions.x, center.y * treeBase.leafDimensions.y, center.z * treeBase.leafDimensions.z);
        Vector3 worldBoundsDimensions = new Vector3(cellsAccross * treeBase.leafDimensions.x, cellsAccross * treeBase.leafDimensions.y, cellsAccross * treeBase.leafDimensions.z);
        worldBounds = new Bounds(worldBoundsCenter, worldBoundsDimensions);
    }

    public abstract OctreeEntry<T> GetAt(Vector3i point);

    public abstract void SetAt(Vector3i point, T value);

    public abstract bool RemoveAt(Vector3i point);

    public abstract void RaycastFind(Ray ray, PriorityQueue<float, OctreeEntry<T>> found);
    

    public bool Contains(Vector3i point)
    {
        Vector3 fish = treeBase.vector3Pool.Catch();

        fish.Set(point.x, point.y, point.z);

        bool contains = bounds.Contains(fish);

        treeBase.vector3Pool.Release(fish);

        return contains;
    }

    protected OctreeChild ChildRelativeTo(Vector3i point)
    {
        int xMod = Convert.ToInt32((point.x - bounds.center.x) >= 0) * OctreeConstants.X_WEIGHT;
        int yMod = Convert.ToInt32((point.y - bounds.center.y) >= 0) * OctreeConstants.Y_WEIGHT;
        int zMod = Convert.ToInt32((point.z - bounds.center.z) >= 0) * OctreeConstants.Z_WEIGHT;

        return (OctreeChild)(xMod + yMod + zMod);
    }

    protected Vector3i CenterOfChildIndex(int childIndex)
    {
        // Represent each direction on a range of [0, 1]. 0 being negaive, 1 being positive
        int zDirection = Convert.ToInt32(childIndex >= OctreeConstants.Z_WEIGHT);
        childIndex -= zDirection * OctreeConstants.Z_WEIGHT;
        int yDirection = Convert.ToInt32(childIndex >= OctreeConstants.Y_WEIGHT);
        childIndex -= yDirection * OctreeConstants.Y_WEIGHT;
        int xDirection = Convert.ToInt32(childIndex >= OctreeConstants.X_WEIGHT);
        childIndex -= xDirection * OctreeConstants.X_WEIGHT;
        // Convert the range from [0, 1] to [-1, 1]
        zDirection = (zDirection * 2) - 1;
        yDirection = (yDirection * 2) - 1;
        xDirection = (xDirection * 2) - 1;

        float halfCellsAccross = treeBase.halfCellsAccrossAtLevel[Level];

        int newNodeCenterZ = (int)(bounds.center.z + (zDirection * halfCellsAccross));
        int newNodeCenterY = (int)(bounds.center.y + (yDirection * halfCellsAccross));
        int newNodeCenterX = (int)(bounds.center.x + (xDirection * halfCellsAccross));

        return new Vector3i(newNodeCenterX, newNodeCenterY, newNodeCenterZ);
    }

    protected bool HasChildren()
    {
        return childCount != 0;
    }

    public int GetCenterX()
    {
        return (int)bounds.center.x;
    }

    public int GetCenterY()
    {
        return (int)bounds.center.y;
    }

    public int GetCenterZ()
    {
        return (int)bounds.center.z;
    }
}
