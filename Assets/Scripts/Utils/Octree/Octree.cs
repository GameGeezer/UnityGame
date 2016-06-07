using UnityEngine;
using System.Collections;
using System;

public class Octree<T> {

    private OctreeBodyNode<T> root;

    public Pool<OctreeEntry<T>> entryPool = new Pool<OctreeEntry<T>>();
    public Pool<OctreeBodyNode<T>> bodyNodePool = new Pool<OctreeBodyNode<T>>();
    public Pool<OctreeLeafNode<T>> leafNodePool = new Pool<OctreeLeafNode<T>>();

    public Octree()
    {
        OctreeBodyNode<T> rootNode = bodyNodePool.Catch();

        root = rootNode.ReInitialize(this, new Vector3i(0, 0, 0), 1);
    }

    public void SetAt(Vector3 point, T value)
    {
        FloorVector(point);

        if (root.Contains(point))
        {
            root.SetAt(point, value);
        }
        else
        {
            GrowTowards(point);

            SetAt(point, value);
        }
    }

    public T GetAt(Vector3 point)
    {
        FloorVector(point);

        if (root.Contains(point))
        {
            return root.GetAt(point);
        }
        else
        {
            return default(T);
        }
    }

    public void RayCastFind(Ray ray, PriorityQueue<T> found)
    {
        root.RaycastFind(ray, found);
    }

    public void Remove(Vector3 point)
    {
        FloorVector(point);

        if (root.Contains(point))
        {
            root.RemoveAt(point);
        }
    }

    private void GrowTowards(Vector3 point)
    {
        int xDirection = Convert.ToInt32((point.x - root.GetCenterX()) < 0);
        int yDirection = Convert.ToInt32((point.y - root.GetCenterY()) < 0);
        int zDirection = Convert.ToInt32((point.z - root.GetCenterZ()) < 0);

        int xIndexMod = xDirection * OctreeConstants.X_WEIGHT;
        int yIndexMod = yDirection * OctreeConstants.Y_WEIGHT;
        int zIndexMod = zDirection * OctreeConstants.Z_WEIGHT;

        OctreeChild moveRootTo = (OctreeChild)(xIndexMod + yIndexMod + zIndexMod);

        xDirection = Convert.ToInt32((point.x - root.GetCenterX()) >= 0);
        yDirection = Convert.ToInt32((point.y - root.GetCenterY()) >= 0);
        zDirection = Convert.ToInt32((point.z - root.GetCenterZ()) >= 0);

        int rootCenterX = ((xDirection * 2) - 1) * (int)root.HalfCellsAccross;
        int rootCenterY = ((yDirection * 2) - 1) * (int)root.HalfCellsAccross;
        int rootCenterZ = ((zDirection * 2) - 1) * (int)root.HalfCellsAccross;

        OctreeBodyNode<T> newRootNode = bodyNodePool.Catch();

        newRootNode.ReInitialize(this, new Vector3i(rootCenterX, rootCenterY, rootCenterZ), root.Level + 1);

        newRootNode.PlaceChild(moveRootTo, root);

        root = newRootNode;
    }

    private void FloorVector(Vector3 vec)
    {
        vec.x = Mathf.Floor(vec.x);

        vec.y = Mathf.Floor(vec.y);

        vec.z = Mathf.Floor(vec.z);
    }
}
