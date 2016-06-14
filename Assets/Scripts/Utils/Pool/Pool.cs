using System.Collections.Generic;

public class Pool<T> where T : new()
{
    private Stack<T> pool = new Stack<T>();

    public Pool()
    {

    }

    public Pool(int size)
    {
        for(int i = 0; i < size; ++i)
        {
            pool.Push(new T());
        }
    }

    public T Catch()
    {
        T fish = pool.Count > 0 ? pool.Pop() : new T();

        return fish;
    }

    public void Release(T fish)
    {
        pool.Push(fish);
    }

    public void ReleaseAll(T[] fish)
    {
        for (int i = 0; i < fish.Length; ++i)
        {
            pool.Push(fish[i]);
        }
    }
}
