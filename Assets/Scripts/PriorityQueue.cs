using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;

[Serializable()]
public struct HeapEntry<T>
{
    private T item;
    private IComparable priority;
    public HeapEntry(T item, IComparable priority)
    {
        this.item = item;
        this.priority = priority;
    }
    public T Item
    {
        get { return item; }
    }
    public IComparable Priority
    {
        get { return priority; }
    }
    public void Clear()
    {
        item = default(T);
        priority = null;
    }
}

[Serializable()]
public class PriorityQueue<T> : ICollection, ISerializable
{
    private int count;
    private int capacity;
    private int version;
    private HeapEntry<T>[] heap;

    private const string capacityName = "capacity";
    private const string countName = "count";
    private const string heapName = "heap";

    public PriorityQueue()
    {
        capacity = 15; // 15 is equal to 4 complete levels
        heap = new HeapEntry<T>[capacity];
    }

    protected PriorityQueue(SerializationInfo info, StreamingContext context)
    {
        capacity = info.GetInt32(capacityName);
        count = info.GetInt32(countName);
        HeapEntry<T>[] heapCopy = (HeapEntry<T>[])info.GetValue(heapName, typeof(HeapEntry<T>[]));
        heap = new HeapEntry<T>[capacity];
        Array.Copy(heapCopy, 0, heap, 0, count);
        version = 0;
    }

    public T Dequeue()
    {
        if (count == 0)
            throw new InvalidOperationException();

        T result = heap[0].Item;
        count--;
        trickleDown(0, heap[count]);
        heap[count].Clear();
        version++;
        return result;
    }

    public void Enqueue(T item, IComparable priority)
    {
        if (priority == null)
            throw new ArgumentNullException("priority");
        if (count == capacity)
            growHeap();
        count++;
        bubbleUp(count - 1, new HeapEntry<T>(item, priority));
        version++;
    }

    private void bubbleUp(int index, HeapEntry<T> he)
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
        HeapEntry<T>[] newHeap = new HeapEntry<T>[capacity];
        System.Array.Copy(heap, 0, newHeap, 0, count);
        heap = newHeap;
    }

    private void trickleDown(int index, HeapEntry<T> he)
    {
        int child = getLeftChild(index);
        while (child < count)
        {
            if (((child + 1) < count) &&
                (heap[child].Priority.CompareTo(heap[child + 1].Priority) < 0))
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
        return new PriorityQueueEnumerator<T>(this);
    }
    #endregion

    #region ISerializable implementation
    [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
    void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue(capacityName, capacity);
        info.AddValue(countName, count);
        HeapEntry<T>[] heapCopy = new HeapEntry<T>[count];
        Array.Copy(heap, 0, heapCopy, 0, count);
        info.AddValue(heapName, heapCopy, typeof(HeapEntry<T>[]));
    }
    #endregion

    #region Priority Queue enumerator
    [Serializable()]
    private class PriorityQueueEnumerator<T> : IEnumerator
    {
        private int index;
        private PriorityQueue<T> pq;
        private int version;

        public PriorityQueueEnumerator(PriorityQueue<T> pq)
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