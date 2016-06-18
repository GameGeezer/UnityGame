using System;
using System.Collections;

public struct HeapEntry<PriorityType, T> where PriorityType : IComparable
{
    private T item;
    private PriorityType priority;

    public HeapEntry(T item, PriorityType priority)
    {
        this.item = item;
        this.priority = priority;
    }

    public T Item
    {
        get { return item; }
    }

    public PriorityType Priority
    {
        get { return priority; }
    }

    public void Clear()
    {
        item = default(T);
        priority = default(PriorityType);
    }
}

public class PriorityQueue<PriorityType, T> : ICollection where PriorityType : IComparable
{
    private int count, capacity, version;

    private HeapEntry<PriorityType, T>[] heap;

    public PriorityQueue()
    {
        capacity = 15;
        heap = new HeapEntry<PriorityType, T>[capacity];
    }

    public PriorityType PeekPriority()
    {
        return heap[0].Priority;
    }

    public void Clear()
    {
        while(count > 0)
        {
            Dequeue();
        }
    }

    public T Dequeue()
    {
        if (count == 0)
            throw new InvalidOperationException();

        T result = heap[0].Item;

        --count;
        trickleDown(0, heap[count]);
        heap[count].Clear();
        ++version;
        return result;
    }

    public void Enqueue(T item, PriorityType priority)
    {
        if (priority == null)
            throw new ArgumentNullException("priority");
        if (count == capacity)
            growHeap();

        ++count;
        bubbleUp(count - 1, new HeapEntry<PriorityType, T>(item, priority));
        ++version;
    }

    private void bubbleUp(int index, HeapEntry<PriorityType, T> he)
    {
        int parent = getParent(index);
        // note: (index > 0) means there is a parent
        while ((index > 0) &&
              (heap[parent].Priority.CompareTo(he.Priority) < 0))
        {
            heap[index] = heap[parent];
            index = parent;
            parent = getParent(index);
        }
        heap[index] = he;
    }

    private int getLeftChild(int index)
    {
        return (index * 2) + 1;
    }

    private int getParent(int index)
    {
        return (index - 1) / 2;
    }

    private void growHeap()
    {
        capacity = (capacity * 2) + 1;
        HeapEntry<PriorityType, T>[] newHeap = new HeapEntry<PriorityType, T>[capacity];
        System.Array.Copy(heap, 0, newHeap, 0, count);
        heap = newHeap;
    }

    private void trickleDown(int index, HeapEntry<PriorityType, T> he)
    {
        int child = getLeftChild(index);
        while (child < count)
        {
            if (((child + 1) < count) && (heap[child].Priority.CompareTo(heap[child + 1].Priority) < 0))
            {
                child++;
            }
            heap[index] = heap[child];
            index = child;
            child = getLeftChild(index);
        }
        bubbleUp(index, he);
    }



    #region ICollection implementation
    public int Count
    {
        get { return count; }
    }

    public void CopyTo(Array array, int index)
    {
        System.Array.Copy(heap, 0, array, index, count);
    }

    public object SyncRoot
    {
        get { return this; }
    }

    public bool IsSynchronized
    {
        get { return false; }
    }
    #endregion


    #region IEnumerable implementation
    public IEnumerator GetEnumerator()
    {
        return new PriorityQueueEnumerator<PriorityType, T>(this);
    }
    #endregion

    #region Priority Queue enumerator
    private class PriorityQueueEnumerator<PriorityType, T> : IEnumerator where PriorityType : IComparable
    {
        private int index;
        private PriorityQueue<PriorityType, T> pq;
        private int version;

        public PriorityQueueEnumerator(PriorityQueue<PriorityType, T> pq)
        {
            this.pq = pq;
            Reset();
        }

        private void checkVersion()
        {
            if (version != pq.version)
                throw new InvalidOperationException();
        }

        #region IEnumerator Members

        public void Reset()
        {
            index = -1;
            version = pq.version;
        }

        public object Current
        {
            get
            {
                checkVersion();
                return pq.heap[index].Item;
            }
        }

        public bool MoveNext()
        {
            checkVersion();
            if (index + 1 == pq.count)
                return false;
            index++;
            return true;
        }

        #endregion
    }
    #endregion

}