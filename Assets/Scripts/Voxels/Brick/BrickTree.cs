using System;
using UnityEngine;

public class BrickTree
{
    private SafeOctree<Brick> octree;

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
        this.noise = noise;

        BrickDimensionX = (int)Math.Pow(2, resolutionX);
        BrickDimensionY = (int)Math.Pow(2, resolutionY);
        BrickDimensionZ = (int)Math.Pow(2, resolutionZ);

        octree = new SafeOctree<Brick>(new Vector3(BrickDimensionX, BrickDimensionY, BrickDimensionZ), new Vector3i(0, 0, 0));

        BrickAndModX = BrickDimensionX - 1;
        BrickAndModY = BrickDimensionY - 1;
        BrickAndModZ = BrickDimensionZ - 1;

        pool = new BrickPool(new Vector3i(resolutionX, resolutionY, resolutionZ));
    }

    public void RaycastFind(Ray ray, PriorityQueue<OctreeEntry<Brick>, float> found)
    {
        octree.RayCastFind(ray, found);
    }

    public void DrawWireFrame()
    {
        octree.DrawWireFrame();
    }

    public OctreeEntry<Brick> GetAt(int x, int y, int z)
    {
        Vector3i dummyVec = new Vector3i(x, y, z);

        OctreeEntry<Brick> entry = octree.GetAt(dummyVec);

        if (entry != null)
        {
            return entry;
        }

        Brick brick;
        lock(pool)
        {
             brick = pool.Catch();
        }
       

        brick.fillWithNoise(x * BrickDimensionX, y * BrickDimensionY, z * BrickDimensionZ, noise);

        entry = octree.SetAtIfNull(dummyVec, brick);

        return entry;
    }

    public Brick RemoveAt(int x, int y, int z)
    {
        Vector3i dummyVec = new Vector3i(x, y, z);

        return octree.RemoveAt(dummyVec);
    }

    public byte GetVoxelAt(int x, int y, int z)
    {
        int localX = FindLocalX(x);
        int localY = FindLocalY(y);
        int localZ = FindLocalZ(z);

        int brickX = x / BrickDimensionX;
        int brickY = y / BrickDimensionY;
        int brickZ = z / BrickDimensionZ;

        Brick brick = GetAt(brickX, brickY, brickZ).entry;
    
        return brick.GetValue(localX, localY, localZ);  
    }

    public void SetVoxelAt(int x, int y, int z, byte value)
    {
        int localX = FindLocalX(x);
        int localY = FindLocalY(y);
        int localZ = FindLocalZ(z);

        int brickX = x / BrickDimensionX;
        int brickY = y / BrickDimensionY;
        int brickZ = z / BrickDimensionZ;

        Brick brick = GetAt(brickX, brickY, brickZ).entry;

        lock (brick)
        {
            brick.SetValue(localX, localY, localZ, value);
        }
    }

    public int FindLocalX(int x)
    {
        return x & (BrickDimensionX - 1);
    }

    public int FindLocalY(int y)
    {
        return y & (BrickDimensionY - 1);
    }

    public int FindLocalZ(int z)
    {
        return z & (BrickDimensionZ - 1);
    }
}
