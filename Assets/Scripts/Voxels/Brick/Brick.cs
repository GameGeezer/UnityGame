using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

/*
    Bricks are groups of voxels
 */
public class Brick : Grid3D<byte> {

    public Brick(int resolutionX, int resolutionY, int resolutionZ) : base((int)Math.Pow(2, resolutionX), (int)Math.Pow(2, resolutionY), (int)Math.Pow(2, resolutionZ))
    {
        
    }

    public void CleanEdges()
    {
        int xEnd = width - 1;
        int yEnd = height - 1;
        int zEnd = depth - 1;
        for (int x = 0; x < width; ++x)
        {
            for (int y = 0; y < height; ++y)
            {
                SetValue(x, y, 0, 0);
                SetValue(x, y, zEnd, 0);
            }
        }

        for (int x = 0; x < width; ++x)
        {
            for (int z = 0; z < depth; ++z)
            {
                SetValue(x, 0, z, 0);
                SetValue(x, yEnd, z, 0);
            }
        }

        for (int y = 0; y < height; ++y)
        {
            for (int z = 0; z < depth; ++z)
            {
                SetValue(0, y, z, 0);
                SetValue(xEnd, y, z, 0);
            }
        }
    }

    public void fillWithNoise(int offsetX, int offsetY, int offsetZ, Noise2D heightNoise)
    {
        for (int x = 0; x < width; ++x)
        {
            for (int z = 0; z < depth; ++z)
            {
                int noiseHeight = heightNoise.generate(offsetX + x, offsetZ + z);

                for (int y = 0; y < height; ++y)
                {
                    if (offsetY + y < noiseHeight)
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
