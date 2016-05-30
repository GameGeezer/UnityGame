﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class BrickTreeCacheFilter
{
    private Dictionary<string, Brick> bricks = new Dictionary<string, Brick>();
    private BrickTree tree;

    public BrickTreeCacheFilter(BrickTree tree)
    {
        this.tree = tree;
    }

    public Brick GetAt(int x, int y, int z)
    {
        string key = HashPosition(x, y, z);

        if(bricks.ContainsKey(key))
        {
            return bricks[key];
        }

        Brick brick = tree.GetAt(x, y, z);

        bricks.Add(key, brick);

        return brick;
    }

    public void Clear()
    {
        bricks.Clear();
    }

    public int GetVoxelAt(int x, int y, int z)
    {
        int localX = tree.FindLocalX(x);
        int localY = tree.FindLocalY(y);
        int localZ = tree.FindLocalZ(z);

        int brickX = x / tree.BrickAndModX;
        int brickY = y / tree.BrickAndModY;
        int brickZ = z / tree.BrickAndModZ;

        Brick brick = GetAt(brickX, brickY, brickZ);

        return brick.GetValue(localX, localY, localZ);
    }

    private string HashPosition(int x, int y, int z)
    {
        return x.ToString() + y.ToString() + z.ToString();
    }
}
