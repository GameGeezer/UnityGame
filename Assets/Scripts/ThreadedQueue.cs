using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ThreadedQueue<TType>
{
    private Queue<TType> _queue;
    private object _queueLock;

    public ThreadedQueue()
    {
        _queue = new Queue<TType>();
        _queueLock = new object();
    }

    public void Enqueue(TType data)
    {
        lock (_queueLock)
        {
            // do not allow duplicates
            if (!_queue.Contains(data))
            {
                _queue.Enqueue(data);
            }

        }
    }

    public bool TryDequeue(out TType data)
    {
        data = default(TType);
        bool success = false;
        lock (_queueLock)
        {
            if (_queue.Count > 0)
            {
                data = _queue.Dequeue();
                success = true;
            }
        }
        return success;
    }

    public int Count()
    {
        lock (_queueLock)
        {
            return _queue.Count;
        }
    }
}