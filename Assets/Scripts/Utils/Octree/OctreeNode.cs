using System;
using UnityEngine;

public abstract class OctreeNode<T> {

    public int level { get; private set; }

    public int childCount { get; protected set; }

    public AABBI cellsCovered;

    public Bounds worldBounds;

    protected Octree<T> treeBase;

    private Vector3 center;

    public OctreeNode()
    {
        cellsCovered = new AABBI();

        worldBounds = new Bounds();

        center = new Vector3();
    }

    protected void BaseInitialize(Octree<T> treeBase, int xMinimum, int yMinimum, int zMinimum, int level)
    {
        this.level = level;

        this.childCount = 0;

        this.treeBase = treeBase;

        // Find the cells accross for  a node at this level

        float cellsAccross = treeBase.cellsAccrossAtLevel[level];

        float halfCellsAccross = treeBase.halfCellsAccrossAtLevel[level];

        // Set the local bounds

        int xMaximum = (int)(xMinimum + cellsAccross);

        int yMaximum = (int)(yMinimum + cellsAccross);

        int zMaximum = (int)(zMinimum + cellsAccross);

        cellsCovered.Set(xMinimum, yMinimum, zMinimum, xMaximum - 1, yMaximum - 1, zMaximum - 1);

        // Set the center

        center.Set(xMinimum + halfCellsAccross, yMinimum + halfCellsAccross, zMinimum + halfCellsAccross);

        // Set the world bounds

        float xWorldMinimum = xMinimum * treeBase.leafDimensions.x;

        float yWorldMinimum = yMinimum * treeBase.leafDimensions.y;

        float zWorldMinimum = zMinimum * treeBase.leafDimensions.z;

        float xWorldMaximum = xMaximum * treeBase.leafDimensions.x;

        float yWorldMaximum = yMaximum * treeBase.leafDimensions.y;

        float zWorldMaximum = zMaximum * treeBase.leafDimensions.z;

        worldBounds.SetMinMax(new Vector3(xWorldMinimum, yWorldMinimum, zWorldMinimum), new Vector3(xWorldMaximum, yWorldMaximum, zWorldMaximum));
    }

    public abstract OctreeEntry<T> SetAt(int x, int y, int z, T value);

    public abstract void RemoveAt(int x, int y, int z);

    public abstract void RaycastFind(Ray ray, PriorityQueue<OctreeEntry<T>, float> found);

    public abstract void DrawGizmos();

    public bool Contains(int x, int y, int z)
    {
        return cellsCovered.Contains(x, y, z);
    }

    ///<summary>
    ///Finds what direction a point is in in relation to the center
    ///</summary>
    public void DirectionTowardsPoint(int x, int y, int z, out int xDirection, out int yDirection, out int zDirection)
    {
        xDirection = Convert.ToInt32((x - cellsCovered.low.x) >= 0);

        yDirection = Convert.ToInt32((y - cellsCovered.low.y) >= 0);

        zDirection = Convert.ToInt32((z - cellsCovered.low.z) >= 0);
    }

    ///<summary>
    ///Finds the child index that would contain the passed point
    ///</summary>
    ///<remarks>
    ///The node must encompass the point to begin with
    ///</remarks>
    public int ChildRelativeTo(int x, int y, int z)
    {
        int xMod = Convert.ToInt32(x >= center.x) * OctreeConstants.X_WEIGHT;

        int yMod = Convert.ToInt32(y >= center.y) * OctreeConstants.Y_WEIGHT;

        int zMod = Convert.ToInt32(z >= center.z) * OctreeConstants.Z_WEIGHT;

        return xMod + yMod + zMod;
    }


    ///<summary>
    ///Takes a child index and returns the 4 point direction along each axis to it
    ///</summary>
    public void IndexToDirection(int index, out int xDirection, out int yDirection, out int zDirection)
    {
        zDirection = Convert.ToInt32(index >= OctreeConstants.Z_WEIGHT);

        index -= zDirection * OctreeConstants.Z_WEIGHT;

        yDirection = Convert.ToInt32(index >= OctreeConstants.Y_WEIGHT);

        index -= yDirection * OctreeConstants.Y_WEIGHT;

        xDirection = Convert.ToInt32(index >= OctreeConstants.X_WEIGHT);

        index -= xDirection * OctreeConstants.X_WEIGHT;
    }

    ///<summary>
    ///Finds the minimum bound of the child at index
    ///</summary>
    protected void MinOfChildIndex(int childIndex, out int xMinimum, out int yMinimum, out int zMinimum)
    {
        int xDirection, yDirection, zDirection;
        // Represent each direction on a range of [0, 1]. 0 being negaive, 1 being positive
        IndexToDirection(childIndex, out xDirection, out yDirection, out zDirection);

        float halfCellsAccross = treeBase.halfCellsAccrossAtLevel[level];

        xMinimum = (int)(cellsCovered.low.x + (xDirection * halfCellsAccross));

        yMinimum = (int)(cellsCovered.low.y + (yDirection * halfCellsAccross));

        zMinimum = (int)(cellsCovered.low.z + (zDirection * halfCellsAccross));
    }

    ///<summary>
    ///Are any of the children nodes set?
    ///</summary>
    protected bool HasChildren()
    {
        return childCount != 0;
    }
}
