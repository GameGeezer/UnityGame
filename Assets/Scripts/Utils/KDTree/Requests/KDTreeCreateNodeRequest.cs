using System;
using System.Collections.Generic;

public class KDTreeCreateNodeRequest<T> : Request
{
    private int start, length;
    private List<KDTreeEntry<T>> nodes;
    private KDTree<T> tree;
    private KDTreeNode<T> node;

    public void Initialize(KDTreeNode<T> node, int start, int length, List<KDTreeEntry<T>> nodes, KDTree<T> tree)
    {
        this.node = node;
        this.start = start;
        this.length = length;
        this.nodes = nodes;
        this.tree = tree;
    }

    public void Perform()
    {
        node.Initialize(start, length, nodes, tree);
    }

    public void PostPerformance()
    {
        
    }

    public void PrePerformance()
    {
        
    }
}
