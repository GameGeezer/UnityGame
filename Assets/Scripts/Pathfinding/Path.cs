using System;
using System.Collections.Generic;
using UnityEngine;

public class Path
{
    private Queue<PathfindingGraphNode> nodes = new Queue<PathfindingGraphNode>();

    public Path()
    {

    }

    public void DrawGizmos()
    {
        foreach(var node in nodes)
        {
            Gizmos.color = Color.magenta;
            node.DrawGizmos();
        }
    }

    public void AddNode(PathfindingGraphNode node)
    {
        nodes.Enqueue(node);
    }

    public PathfindingGraphNode GetNext()
    {
        return nodes.Dequeue();
    }

    public bool HasNext()
    {
        return nodes.Count > 0;
    }
}
