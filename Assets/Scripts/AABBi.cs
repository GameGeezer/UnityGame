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
}
