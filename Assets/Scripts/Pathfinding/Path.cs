using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Path
{
    private Queue<PathfindingGraphNode> nodes = new Queue<PathfindingGraphNode>();

    public Path()
    {

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
