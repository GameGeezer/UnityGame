using UnityEngine;
using System.Collections;

public class AABBi {

    public Vector3i Lower { get; private set; }
    public Vector3i Upper { get; private set; }

    public AABBi(Vector3i lower, Vector3i upper)
    {
        this.Lower = lower;
        this.Upper = upper;
    }

    public bool Contains(Vector3i cell)
    {
        Vector3i lowerMinCell = Lower - cell;

        Vector3i upperMinCell = Upper - cell;

        bool lowerLegal = lowerMinCell.x <= 0 && lowerMinCell.y <= 0 && lowerMinCell.z <= 0;

        bool upperLegal = upperMinCell.x >= 0 && upperMinCell.y >= 0 && upperMinCell.z >= 0;

        return lowerLegal && upperLegal;
    }

    public bool Contains(int x, int y, int z)
    {
        int lowerMinCellX = Lower.x - x;
        int lowerMinCellY = Lower.y - y;
        int lowerMinCellZ = Lower.z - z;

        int upperMinCellX = Upper.x - x;
        int upperMinCellY = Upper.y - y;
        int upperMinCellZ = Upper.z - z;

        bool lowerLegal = lowerMinCellX <= 0 && lowerMinCellY <= 0 && lowerMinCellZ <= 0;

        bool upperLegal = upperMinCellX >= 0 && upperMinCellY >= 0 && upperMinCellZ >= 0;

        return lowerLegal && upperLegal;
    }
}
