

using UnityEngine;

public abstract class KDTreeNode<T>
{
    public abstract int NearestNeighbor(Vector3 position);
}
