using System.Collections.Generic;
using UnityEngine;

public class KDTempNode<T> : KDTreeNode<T>
{
    public override int NearestNeighbor(Vector3 position)
    {
        return 0;
    }

    public override void Initialize(int startOrValue, int length, List<KDTreeEntry<T>> nodes, KDTree<T> tree)
    {
        
    }

    public override void Clear(KDTreePoolParty<T> poolParty)
    {
        
    }
}