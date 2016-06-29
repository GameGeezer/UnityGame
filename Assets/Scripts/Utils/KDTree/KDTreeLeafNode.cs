
using System.Collections.Generic;
using UnityEngine;

public class KDTreeLeafNode<T> : KDTreeNode<T>
{
    private int index;

    public KDTreeLeafNode()
    {

    }

    public override int NearestNeighbor(Vector3 position)
    {
        return index;
    }

    public override void Initialize(int startOrValue, int length, List<KDTreeEntry<T>> nodes, KDTree<T> tree)
    {
        this.index = startOrValue;
    }

    public override void Clear(KDTreePoolParty<T> poolParty)
    {
        poolParty.leafNodePool.Release(this);
    }
}
