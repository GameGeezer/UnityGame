using UnityEngine;
using System.Collections;

/*
    Bricks are groups of voxels
 */
public class Brick : Grid3D<int> {

    public Brick(int dimX, int dimY, int dimZ) : base(dimX, dimY, dimZ)
    {
        
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
