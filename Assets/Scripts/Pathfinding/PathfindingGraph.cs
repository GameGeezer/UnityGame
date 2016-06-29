using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class PathfindingGraph
{
    private Dictionary<Vector3i, PathfindingGraphNode> nodes = new Dictionary<Vector3i, PathfindingGraphNode>();

    private KDTree<PathfindingGraphNode> nodeTree = new KDTree<PathfindingGraphNode>();

    public Pool<PathfindingGraphNode> graphNodePool = new Pool<PathfindingGraphNode>();

    public PathfindingGraph()
    {
        CreateGrid(16);
    }

    public Path FindPath(Vector3 from, Vector3 to)
    {
        KDTreeEntry<PathfindingGraphNode> entryFrom = Nearest(from);

        KDTreeEntry<PathfindingGraphNode> entryTo = Nearest(to);

        Path path = new Path();

        entryFrom.value.FindPath(entryTo.value, path);

        return path;
    }

    public PathfindingGraphNode Place(int x, int y, int z)
    {
        PathfindingGraphNode node = graphNodePool.Catch();

        node.Initialize(x, y, z);

        nodeTree.Insert(node, new Vector3(x, y, z));

        nodes.Add(new Vector3i(x, y, z), node);

        return node;
    }

    public void Refresh()
    {
        nodeTree.Refresh();
    }

    public void Clear()
    {
        nodeTree.Clear();
    }

    public PathfindingGraphNode Get(Vector3i location)
    {
        return nodes[location];
    }

    public KDTreeEntry<PathfindingGraphNode> Nearest(Vector3 position)
    {
        return nodeTree.NearestNeighbor(position);
    }

    public void DrawGizmos()
    {
        foreach (var node in nodes)
        {
            node.Value.DrawGizmos();
        }
    }

    public void CreateGrid(int res)
    {
        for(int x = 0; x < res; ++x)
        {
            for (int y = 0; y < res; ++y)
            {
                Place(x, 16, y);
            }
        }
        Vector3i dummy = new Vector3i();
        for (int x = 1; x < res - 1; ++x)
        {
            for (int y = 1; y < res - 1; ++y)
            {
                PathfindingGraphNode node = Get(dummy.Set(x, 16, y));

                node.LinkToNode(Get(dummy.Set(x - 1, 16, y - 1)));
                node.LinkToNode(Get(dummy.Set(x - 1, 16, y)));
                node.LinkToNode(Get(dummy.Set(x - 1, 16, y + 1)));

                node.LinkToNode(Get(dummy.Set(x, 16, y + 1)));
                node.LinkToNode(Get(dummy.Set(x + 1, 16, y + 1)));
                node.LinkToNode(Get(dummy.Set(x + 1, 16, y)));
                node.LinkToNode(Get(dummy.Set(x + 1, 16, y - 1)));
                node.LinkToNode(Get(dummy.Set(x, 16, y - 1)));

                node.Refresh();
            }
        }
    }
}
