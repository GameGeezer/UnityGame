using UnityEngine;
using System.Collections;

public class Grid3D<T> {

    T[,,] data;

    public Grid3D(int width, int height, int depth)
    {
        data = new T[width, height, depth];
    }

    public void fill(T value)
    {
        for(int x = 0; x < GetWidth(); ++x)
        {
            for (int y = 0; y < GetHeight(); ++y)
            {
                for (int z = 0; z < GetDepth(); ++z)
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

    public int GetWidth()
    {
        return data.GetLength(0);
    }

    public int GetHeight()
    {
        return data.GetLength(1);
    }

    public int GetDepth()
    {
        return data.GetLength(2);
    }
}
