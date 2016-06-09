using System;
using UnityEngine;

public class BrickTree
{
    private Octree<Brick> octree;
    private Noise2D noise;
    private BrickPool pool;

    public int BrickAndModX { get; private set; }
    public int BrickAndModY { get; private set; }
    public int BrickAndModZ { get; private set; }

    public int BrickDimensionX { get; private set; }
    public int BrickDimensionY { get; private set; }
    public int BrickDimensionZ { get; private set; }

    private Vector3i dummyVec = new Vector3i();

    public BrickTree(int resolutionX, int resolutionY, int resolutionZ, Noise2D noise)
    {
        this.noise = noise;

        BrickDimensionX = (int)Math.Pow(2, resolutionX);
        BrickDimensionY = (int)Math.Pow(2, resolutionY);
        BrickDimensionZ = (int)Math.Pow(2, resolutionZ);

        octree = new Octree<Brick>(new Vector3(BrickDimensionX, BrickDimensionY, BrickDimensionZ), new Vector3i(0, 0, 0));

        BrickAndModX = BrickDimensionX - 1;
        BrickAndModY = BrickDimensionY - 1;
        BrickAndModZ = BrickDimensionZ - 1;

        pool = new BrickPool(new Vector3i(BrickDimensionX, BrickDimensionY, BrickDimensionZ));
    }

    public void RaycastFind(Ray ray, PriorityQueue<OctreeEntry<Brick>> found)
    {
        octree.RayCastFind(ray, found);
    }

    public Brick GetAt(int x, int y, int z)
    {
        dummyVec.Set(x * 2, y * 2, z * 2);

        OctreeEntry<Brick> brickEntry = octree.GetAt(dummyVec);

        Brick brick = brickEntry == null ? null : brickEntry.entry;

        if (brick != null)
        {
            return brick;
        }

        brick = pool.Catch();

        brick.fillWithNoise(x * BrickDimensionX, y * BrickDimensionY, z * BrickDimensionZ, noise);

        octree.SetAt(dummyVec, brick);

        return brick;
    }

    public int FindLocalX(int x)
    {
        return x % BrickDimensionX;
    }

    public int FindLocalY(int y)
    {
        return y % BrickDimensionY;
    }

    public int FindLocalZ(int z)
    {
        return z % BrickDimensionZ;
    }
}
