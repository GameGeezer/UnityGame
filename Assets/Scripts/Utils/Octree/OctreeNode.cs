using System;
using UnityEngine;

public abstract class OctreeNode<T> {

    public int Level { get; private set; }

    protected int childCount { get; set; }

    public Bounds bounds, worldBounds;

    protected Octree<T> treeBase;

    private Vector3 center = new Vector3();

    public OctreeNode()
    {
        bounds = new Bounds();
        worldBounds = new Bounds();
    }

    protected void Initialize(Octree<T> treeBase, Vector3i min, int level)
    {
        this.Level = level;
        this.treeBase = treeBase;
        this.childCount = 0;
        // Get the size this cell is accross
        float cellsAccross = treeBase.cellsAccrossAtLevel[level];
        float halfCellsAccross = treeBase.halfCellsAccrossAtLevel[level];

        center = new Vector3(min.x + halfCellsAccross, min.y + halfCellsAccross, min.z + halfCellsAccross);
        float maxX = min.x + cellsAccross;
        float maxY = min.y + cellsAccross;
        float maxZ = min.z + cellsAccross;
        bounds.SetMinMax(new Vector3(min.x , min.y, min.z), new Vector3(maxX - 1, maxY - 1, maxZ - 1));

        worldBounds.SetMinMax(new Vector3(min.x * treeBase.leafDimensions.x, min.y * treeBase.leafDimensions.y, min.z * treeBase.leafDimensions.z), new Vector3(maxX * treeBase.leafDimensions.x, maxY * treeBase.leafDimensions.y, maxZ * treeBase.leafDimensions.z));
    }

    public abstract void RaycastFind(Ray ray, PriorityQueue<float, OctreeEntry<T>> found);

    public abstract OctreeEntry<T> GetAt(Vector3i point);

    public abstract void SetAt(Vector3i point, T value);

    public abstract bool RemoveAt(Vector3i point);

    public abstract void Draw();
    

    public bool Contains(Vector3i point)
    {
        Vector3 fish = new Vector3();

        fish.Set(point.x, point.y, point.z);

        bool contains = bounds.Contains(fish);

        //treeBase.vector3Pool.Release(fish);

        return contains;
    }

    public void DirectionTowardsPoint(Vector3i point, out int xDirection, out int yDirection, out int zDirection)
    {
        xDirection = Convert.ToInt32((point.x - center.x) < 0);
        yDirection = Convert.ToInt32((point.y - center.y) < 0);
        zDirection = Convert.ToInt32((point.z - center.z) < 0);
    }

    public OctreeChild ChildRelativeTo(Vector3i point)
    {
        int xMod = Convert.ToInt32(point.x >= center.x) * OctreeConstants.X_WEIGHT;
        int yMod = Convert.ToInt32(point.y >= center.y) * OctreeConstants.Y_WEIGHT;
        int zMod = Convert.ToInt32(point.z >= center.z) * OctreeConstants.Z_WEIGHT;

        return (OctreeChild)(xMod + yMod + zMod);
    }

    public void IndexToDirection(int index, out int xDirection, out int yDirection, out int zDirection)
    {
        zDirection = Convert.ToInt32(index >= OctreeConstants.Z_WEIGHT);
        index -= zDirection * OctreeConstants.Z_WEIGHT;
        yDirection = Convert.ToInt32(index >= OctreeConstants.Y_WEIGHT);
        index -= yDirection * OctreeConstants.Y_WEIGHT;
        xDirection = Convert.ToInt32(index >= OctreeConstants.X_WEIGHT);
        index -= xDirection * OctreeConstants.X_WEIGHT;
    }

    protected Vector3i MinOfChildIndex(int childIndex)
    {
        // Represent each direction on a range of [0, 1]. 0 being negaive, 1 being positive
        int xDirection, yDirection, zDirection;
        IndexToDirection(childIndex, out xDirection, out yDirection, out zDirection);

        float halfCellsAccross = treeBase.halfCellsAccrossAtLevel[Level];

        int newNodeCenterZ = (int)(bounds.min.z + (zDirection * halfCellsAccross));
        int newNodeCenterY = (int)(bounds.min.y + (yDirection * halfCellsAccross));
        int newNodeCenterX = (int)(bounds.min.x + (xDirection * halfCellsAccross));

        return new Vector3i(newNodeCenterX, newNodeCenterY, newNodeCenterZ);
    }

    protected bool HasChildren()
    {
        return childCount != 0;
    }
}
