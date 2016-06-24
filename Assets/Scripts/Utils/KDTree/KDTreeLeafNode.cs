using System;
using UnityEngine;

public class KDTreeLeafNode<T> : KDTreeNode<T>
{
    private int index;

    public override int NearestNeighbor(Vector3 position)
    {
        return index;
    }

    public void Initialize(int index)
    {
        this.index = index;
    }
}
