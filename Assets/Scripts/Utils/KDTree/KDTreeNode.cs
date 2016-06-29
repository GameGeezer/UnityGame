

using System.Collections.Generic;
using UnityEngine;

public abstract class KDTreeNode<T>
{
    public abstract void Initialize(int startOrValue, int length, List<KDTreeEntry<T>> nodes, KDTree<T> tree);

    public abstract int NearestNeighbor(Vector3 position);

    public abstract void Clear(KDTreePoolParty<T> poolParty);
}
