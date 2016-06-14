using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class SafePool<T> where T : new()
{
    private Pool<T> pool = new Pool<T>();

    public SafePool()
    {

    }

    public SafePool(int size)
    {
        for (int i = 0; i < size; ++i)
        {
            pool.Release(new T());
        }
    }

    public T Catch()
    {
        lock (pool)
        {
            return pool.Catch();
        }
    }

    public void Release(T chunk)
    {
        lock (pool)
        {
            pool.Release(chunk);
        }
    }

    public void ReleaseAll(T[] fish)
    {
        lock (pool)
        {
            pool.ReleaseAll(fish);
        }
    }
}
