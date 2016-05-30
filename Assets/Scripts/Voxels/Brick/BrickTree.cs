using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class BrickTree
{
    private Octree<Brick> octree = new Octree<Brick>();
    private Noise2D noise;
    private BrickPool pool;

    public int BrickAndModX { get; private set; }
    public int BrickAndModY { get; private set; }
    public int BrickAndModZ { get; private set; }

    public int BrickDimensionX { get; private set; }
    public int BrickDimensionY { get; private set; }
    public int BrickDimensionZ { get; private set; }

    public BrickTree(int resolutionX, int resolutionY, int resolutionZ, Noise2D noise)
    {
        BrickDimensionX = (int)Math.Pow(2, resolutionX);
        BrickDimensionY = (int)Math.Pow(2, resolutionY);
        BrickDimensionZ = (int)Math.Pow(2, resolutionZ);

        BrickAndModX = BrickDimensionX - 1;
        BrickAndModY = BrickDimensionY - 1;
        BrickAndModZ = BrickDimensionZ - 1;

        pool = new BrickPool(new Vector3i(BrickDimensionX, BrickDimensionY, BrickDimensionZ));
        this.noise = noise;
    }

    public Brick GetAt(int x, int y, int z)
    {
        Brick brick = octree.GetAt(x, y, z);

        if(brick != null)
        {
            return brick;
        }

        brick = pool.Catch();

        brick.fillWithNoise(x, y, z, noise);

        octree.Place(x, y, z, brick);

        return brick;
    }

    public int FindLocalX(int x)
    {
        return x & BrickAndModX;
    }

    public int FindLocalY(int y)
    {
        return y & BrickAndModY;
    }

    public int FindLocalZ(int z)
    {
        return z & BrickAndModZ;
    }
}
