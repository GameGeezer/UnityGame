using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using Unity.IO.Compression;
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

    public BrickTree(Vector3i resolution, Noise2D noise)
    {
        this.noise = noise;

        BrickDimensionX = (int)Math.Pow(2, resolution.x);
        BrickDimensionY = (int)Math.Pow(2, resolution.y);
        BrickDimensionZ = (int)Math.Pow(2, resolution.z);

        octree = new SafeOctree<Brick>(new Vector3(BrickDimensionX, BrickDimensionY, BrickDimensionZ), new Vector3i(0, 0, 0));

        BrickAndModX = BrickDimensionX - 1;
        BrickAndModY = BrickDimensionY - 1;
        BrickAndModZ = BrickDimensionZ - 1;

        pool = new BrickPool(resolution);
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
        OctreeEntry<Brick> entry = octree.GetAt(x, y, z);

        if (entry != null )
        {
            return entry;
        }

        return AddBrickAt(x, y, z);
    }

    public void RemoveAt(int x, int y, int z)
    {
        octree.RemoveAt(x, y, z);
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

    public void Encode(string filePath)
    {

        MemoryStream brickStream = new MemoryStream();
        DataContractSerializer cellSerializer = new DataContractSerializer(typeof(List<OctreeEntry<Brick>>));

        List<OctreeEntry<Brick>> leafNodes = octree.GetLeafEnumerator();

        cellSerializer.WriteObject(brickStream, leafNodes);

        FileStream fileStream = File.Create(filePath);

        brickStream.Position = 0;
        CompressionUtil.ZipToFile(brickStream, fileStream);

        fileStream.Close();
        brickStream.Close();
    }

    public void Decode(string filePath)
    {
        DataContractSerializer cellSerializer = new DataContractSerializer(typeof(List<OctreeEntry<Brick>>));

        FileStream fileStream = File.Open(filePath, FileMode.Open);
        byte[] bytes = new byte[fileStream.Length];
        fileStream.Read(bytes, 0, (int)fileStream.Length);
        fileStream.Close();

        using (var msi = new MemoryStream(bytes))
        using (var mso = new MemoryStream())
        {
            using (var gs = new GZipStream(msi, CompressionMode.Decompress))
            {
                //gs.CopyTo(mso);
                CompressionUtil.CopyTo(gs, mso);

                mso.Seek(0, SeekOrigin.Begin);
                List<OctreeEntry<Brick>> cell = (List<OctreeEntry<Brick>>)cellSerializer.ReadObject(mso);
            }
            

            
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

    private OctreeEntry<Brick> AddBrickAt(int x, int y, int z)
    {
        Brick brick;
        lock (pool)
        {
            brick = pool.Catch();
        }

        brick.fillWithNoise(x * BrickDimensionX, y * BrickDimensionY, z * BrickDimensionZ, noise);

        return octree.SetAtIfNull(x, y, z, brick);
    }
}
