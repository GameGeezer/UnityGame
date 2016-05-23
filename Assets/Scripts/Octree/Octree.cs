using UnityEngine;
using System.Collections;
using System;

public class Octree<T> {

    private OctreeBodyNode<T> root;

    public Octree()
    {
        root = new OctreeBodyNode<T>(new Vector3i(0, 0, 0), 1);
    }

    public void Place(Vector3i position, T value)
    {
        if(root.Contains(position))
        {
            root.SetAt(position, value);
        }
        else
        {
            BuildRootRelativeTo(position);

            Place(position, value);
        }
    }

    public T GetAt(Vector3i position)
    {
        if (root.Contains(position))
        {
            return root.GetAt(position);
        }
        else
        {
            return default(T);
        }
    }

    private void BuildRootRelativeTo(Vector3i position)
    {
        int xDirection = Convert.ToInt32((position.x - root.Center.x) >= 0);
        int yDirection = Convert.ToInt32((position.y - root.Center.y) >= 0);
        int zDirection = Convert.ToInt32((position.z - root.Center.z) >= 0);

        int xIndexMod = xDirection * OctreeConstants.X_WEIGHT;
        int yIndexMod = yDirection * OctreeConstants.Y_WEIGHT;
        int zIndexMod = zDirection * OctreeConstants.Z_WEIGHT;

        OctreeChild moveRootTo = (OctreeChild)(xIndexMod + yIndexMod + zIndexMod);

        int rootCenterX = ((xDirection * 2) - 1) * root.HalfCellsAccross;
        int rootCenterY = ((yDirection * 2) - 1) * root.HalfCellsAccross;
        int rootCenterZ = ((zDirection * 2) - 1) * root.HalfCellsAccross;

        OctreeBodyNode<T> newRoot = new OctreeBodyNode<T>(new Vector3i(rootCenterX, rootCenterY, rootCenterZ), root.Level + 1);

        newRoot.SetChild(moveRootTo, root);
    }
}
