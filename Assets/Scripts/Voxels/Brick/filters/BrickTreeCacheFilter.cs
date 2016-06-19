using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BrickTreeCacheFilter
{
    private Dictionary<string, Brick> bricks = new Dictionary<string, Brick>();
    private BrickTree tree;

    public BrickTreeCacheFilter(BrickTree tree)
    {
        this.tree = tree;
    }

    public void RaycastFind(Ray ray, PriorityQueue<float, OctreeEntry<Brick>> found)
    {
        tree.RaycastFind(ray, found);
    }

    public Brick GetAt(int x, int y, int z)
    {
        string key = HashPosition(x, y, z);

        if(bricks.ContainsKey(key))
        {
            return bricks[key];
        }

        Brick brick;
        lock (tree)
        {
            brick = tree.GetAt(x, y, z);
        }

        bricks.Add(key, brick);

        return brick;
    }

    public void Clear()
    {
        bricks.Clear();
    }



    private string HashPosition(int x, int y, int z)
    {
        return x.ToString() + y.ToString() + z.ToString();
    }
}
