using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ThreadedQueue<TType>
{
    private Queue<TType> queue;

    public ThreadedQueue()
    {
        queue = new Queue<TType>();
    }

    public void Enqueue(TType data)
    {
        lock (queue)
        {
            // do not allow duplicates
            queue.Enqueue(data);
        }
    }

    public bool TryDequeue(out TType data)
    {
        data = default(TType);
        bool success = false;
        lock (queue)
        {
            if (queue.Count > 0)
            {
                data = queue.Dequeue();
                success = true;
            }
        }
        return success;
    }

    public int Count()
    {
        lock (queue)
        {
            return queue.Count;
        }
    }
}