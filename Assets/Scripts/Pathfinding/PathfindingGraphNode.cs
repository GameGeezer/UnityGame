

using System.Collections.Generic;
using UnityEngine;

public class PathfindingGraphNode
{
    public Vector3 position = new Vector3();

    private KDTree<PathfindingGraphNode> pathNodes = new KDTree<PathfindingGraphNode>();

    public void Initialize(int x, int y, int z)
    {
        position.Set(x, y, z);

        pathNodes.Insert(this, position);
    }

    public void FindPath(PathfindingGraphNode to, Path path)
    {
        path.AddNode(this);

        KDTreeEntry<PathfindingGraphNode> nearest = pathNodes.NearestNeighbor(to.position);

        if(nearest.value.Equals(this) || nearest.value.Equals(to))
        {
            return;
        }

        nearest.value.FindPath(to, path);
    }

    public void LinkToNode(PathfindingGraphNode node)
    {
        pathNodes.Insert(node, new Vector3(node.position.x, node.position.y, node.position.z));
    }

    public void Refresh()
    {
        pathNodes.Refresh();
    }

    public void DrawGizmos()
    {
        foreach (var node in pathNodes.entries)
        {
            Gizmos.DrawLine(position, node.position);
        }
    }
}