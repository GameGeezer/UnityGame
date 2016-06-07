using System;
using UnityEngine;

public abstract class OctreeNode<T> {

    public int Level { get; private set; }

    public float HalfCellsAccross {get; private set;}

    protected Bounds bounds;

    protected int childCount { get; set; }

    protected Octree<T> treeBase;

    public OctreeNode()
    {
        
    }

    protected void Initialize(Octree<T> treeBase, Vector3i center, int level)
    {
        this.treeBase = treeBase;
        childCount = 0;
        this.Level = level;
        float cellsAccross = (int)Mathf.Pow(2, level + 1);
        HalfCellsAccross = (cellsAccross / 2);
        Vector3 boundsCenter = new Vector3(center.x, center.y, center.z);
        Vector3 boundsDimensions = new Vector3(cellsAccross, cellsAccross, cellsAccross);
        bounds = new Bounds(boundsCenter, boundsDimensions);
    }

    public abstract T GetAt(Vector3 point);

    public abstract void RaycastFind(Ray ray, PriorityQueue<T> found);

    public abstract void SetAt(Vector3 point, T value);

    public abstract bool RemoveAt(Vector3 point);

    public bool Contains(Vector3 point)
    {
        return bounds.Contains(point);
    }

    protected OctreeChild ChildRelativeTo(Vector3 point)
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

        int newNodeCenterZ = (int)(bounds.center.z + (zDirection * HalfCellsAccross));
        int newNodeCenterY = (int)(bounds.center.y + (yDirection * HalfCellsAccross));
        int newNodeCenterX = (int)(bounds.center.x + (xDirection * HalfCellsAccross));

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
