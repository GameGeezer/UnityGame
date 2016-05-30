using UnityEngine;
using System.Collections;
using System;

public class Octree<T> {

    private OctreeBodyNode<T> root;

    public Octree()
    {
        root = new OctreeBodyNode<T>(new Vector3i(0, 0, 0), 1);
    }

    public void Place(int x, int y, int z, T value)
    {
        if(root.Contains(x, y, z))
        {
            root.SetAt(x, y, z, value);
        }
        else
        {
            GrowTowards(x, y, z);

            Place(x, y, z, value);
        }
    }

    public T GetAt(int x, int y, int z)
    {
        if (root.Contains(x, y, z))
        {
            return root.GetAt(x, y, z);
        }
        else
        {
            return default(T);
        }
    }

    private void GrowTowards(int x, int y, int z)
    {
        int xDirection = Convert.ToInt32((x - root.Center.x) < 0);
        int yDirection = Convert.ToInt32((y - root.Center.y) < 0);
        int zDirection = Convert.ToInt32((z - root.Center.z) < 0);

        int xIndexMod = xDirection * OctreeConstants.X_WEIGHT;
        int yIndexMod = yDirection * OctreeConstants.Y_WEIGHT;
        int zIndexMod = zDirection * OctreeConstants.Z_WEIGHT;

        OctreeChild moveRootTo = (OctreeChild)(xIndexMod + yIndexMod + zIndexMod);

        xDirection = Convert.ToInt32((x - root.Center.x) >= 0);
        yDirection = Convert.ToInt32((y - root.Center.y) >= 0);
        zDirection = Convert.ToInt32((z - root.Center.z) >= 0);

        int rootCenterX = ((xDirection * 2) - 1) * root.HalfCellsAccross;
        int rootCenterY = ((yDirection * 2) - 1) * root.HalfCellsAccross;
        int rootCenterZ = ((zDirection * 2) - 1) * root.HalfCellsAccross;

        OctreeBodyNode<T> newRoot = new OctreeBodyNode<T>(new Vector3i(rootCenterX, rootCenterY, rootCenterZ), root.Level + 1);

        newRoot.SetChild(moveRootTo, root);

        root = newRoot;
    }
}
