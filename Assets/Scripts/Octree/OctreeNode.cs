using System;
using UnityEngine;

public abstract class OctreeNode<T> {

    public int HalfCellsAccross {get; private set;}

    public AABBi AABB { get; private set; }
    public Vector3i Center { get; private set; }

    public OctreeNode(Vector3i center, int level)
    {
        this.Center = center;
        HalfCellsAccross = (int)Mathf.Pow(2, level) / 2;
        AABB = new AABBi(center - HalfCellsAccross, center + HalfCellsAccross);     
    }

    public abstract T GetAt(Vector3i position);

    public abstract void SetAt(Vector3i position, T value);

    public bool Contains(Vector3i point)
    {
        return AABB.Contains(point);
    }

    protected OctreeChild ChildRelativeTo(Vector3i position)
    {
        int xMod = Convert.ToInt32((position.x - Center.x) >= 0) * OctreeConstants.X_WEIGHT;
        int yMod = Convert.ToInt32((position.y - Center.y) >= 0) * OctreeConstants.Y_WEIGHT;
        int zMod = Convert.ToInt32((position.z - Center.z) >= 0) * OctreeConstants.Z_WEIGHT;

        return (OctreeChild)(xMod + yMod + zMod);
    }

    protected Vector3i CenterOfChildIndex(int currentLevel, int childIndex)
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

        int newNodeCenterZ = Center.z + (zDirection * HalfCellsAccross);
        int newNodeCenterY = Center.y + (yDirection * HalfCellsAccross);
        int newNodeCenterX = Center.x + (xDirection * HalfCellsAccross);

        return new Vector3i(newNodeCenterX, newNodeCenterY, newNodeCenterZ);
    }
}
