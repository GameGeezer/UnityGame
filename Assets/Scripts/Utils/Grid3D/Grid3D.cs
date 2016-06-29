using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class Grid3D<T> {

    public T[,,] data;
    public int width { get; private set; }
    public int height { get; private set; }
    public int depth { get; private set; }

    public Grid3D(int width, int height, int depth)
    {
        this.width = width;
        this.height = height;
        this.depth = depth;

        data = new T[width, height, depth];
    }

    public void fill(T value)
    {
        for(int x = 0; x < width; ++x)
        {
            for (int y = 0; y < height; ++y)
            {
                for (int z = 0; z < depth; ++z)
                {
                    SetValue(x, y, z, value);
                }
            }
        }
    }

    public T GetValue(int x, int y, int z)
    {
        return data[x, y, z];
    }

    public void SetValue(int x, int y, int z, T value)
    {
        data[x, y, z] = value;
    }

    public int DataSizeInBytes()
    {
        return Marshal.SizeOf(typeof(T)) * width * height * depth;
    }
}
