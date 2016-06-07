using UnityEngine;
using System.Collections;

/*
    Bricks are groups of voxels
 */
public class Brick : Grid3D<int> {

    public Brick(int dimX, int dimY, int dimZ) : base(dimX, dimY, dimZ)
    {
        
    }

    public void RaycastCells(Ray ray, PriorityQueue<Vector3i> found, float offsetX, float offsetY, float offsetZ)
    {
        float xMin, yMin, zMin;
        for (int x = 0; x < GetWidth(); ++x)
        {
            for (int z = 0; z < GetDepth(); ++z)
            {
                for (int y = 0; y < GetHeight(); ++y)
                {
                    xMin = x + offsetX;
                    yMin = y + offsetY;
                    zMin = z + offsetZ;
                    if(CollisionUtil.IntersectsBounds(ray, xMin, yMin, zMin, xMin + 1, yMin + 1, zMin + 1))
                    {
                        found.Enqueue(GamePools.Vector3iPool.Catch().Set(x, y, z), 1);
                    }
                }
            }
        }
    }

    public void fillWithNoise(int offsetX, int offsetY, int offsetZ, Noise2D heightNoise)
    {
        for (int x = 0; x < GetWidth(); ++x)
        {
            for (int z = 0; z < GetDepth(); ++z)
            {
                int height = heightNoise.generate(offsetX + x, offsetZ + z);

                for (int y = 0; y < GetHeight(); ++y)
                {
                    if (offsetY + y < height)
                    {
                        SetValue(x, y, z, 1);
                    }
                    else
                    {
                        SetValue(x, y, z, 0);
                    }
                }
            }
        }
    }
}
