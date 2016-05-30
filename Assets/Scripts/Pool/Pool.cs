using System.Collections.Generic;

public class Pool<T> where T : new()
{
    private Stack<T> pool = new Stack<T>();

    public T Catch()
    {
        T fish = pool.Count > 0 ? pool.Pop() : new T();

        return fish;
    }

    public void Release(T fish)
    {
        pool.Push(fish);
    }
}
