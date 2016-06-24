using System;

public class PriorityQueue<EntryType, ComparatorType> where ComparatorType : IComparable
{
    public int Count { get; private set; }

    private Pool<PriorityQueueBodyNode<EntryType, ComparatorType>> nodesPool = new Pool<PriorityQueueBodyNode<EntryType, ComparatorType>>();

    private PriorityQueueTailNode<EntryType, ComparatorType> tail;

    private PriorityQueueHeadNode<EntryType, ComparatorType> head;

    public PriorityQueue()
    {
        tail = new PriorityQueueTailNode<EntryType, ComparatorType>();

        head = new PriorityQueueHeadNode<EntryType, ComparatorType>();

        tail.next = head;

        head.previous = tail;
    }

    public void Clear()
    {
        tail.next = head;

        head.previous = tail;

        Count = 0;
    }

    public void Enqueue(EntryType entry, ComparatorType comparator)
    {
        PriorityQueueBodyNode<EntryType, ComparatorType> fish = nodesPool.Catch();

        fish.Initialize(entry, comparator);

        tail.Bubble(fish);

        ++Count;
    }

    public EntryType Dequeue()
    {
        if (Count == 0)
        {
            return default(EntryType);
        }

        PriorityQueueBodyNode<EntryType, ComparatorType> entry = (PriorityQueueBodyNode<EntryType, ComparatorType>)head.previous;

        entry.previous.next = head;

        head.previous = entry.previous;

        nodesPool.Release(entry);

        --Count;

        return entry.item;
    }

    public bool IsEmpty()
    {
        return Count == 0;
    }

    public ComparatorType PeekAtPriority()
    {
        if (Count == 0)
        {
            return default(ComparatorType);
        }

        PriorityQueueBodyNode<EntryType, ComparatorType> entry = (PriorityQueueBodyNode<EntryType, ComparatorType>)head.previous;

        return entry.priority;
    }
}

abstract class PriorityQueueNode<EntryType, ComparatorType> where ComparatorType : IComparable
{
    public PriorityQueueNode<EntryType, ComparatorType> next { get; set; }

    public PriorityQueueNode<EntryType, ComparatorType> previous { get; set; }

    public abstract void Bubble(PriorityQueueBodyNode<EntryType, ComparatorType> node);
}

class PriorityQueueTailNode<EntryType, ComparatorType> : PriorityQueueNode<EntryType, ComparatorType> where ComparatorType : IComparable
{
    public override void Bubble(PriorityQueueBodyNode<EntryType, ComparatorType> node)
    {
        next.Bubble(node);
    }
}

class PriorityQueueHeadNode<EntryType, ComparatorType> : PriorityQueueNode<EntryType, ComparatorType> where ComparatorType : IComparable
{
    public override void Bubble(PriorityQueueBodyNode<EntryType, ComparatorType> node)
    {
        previous.next = node;

        node.previous = previous;

        node.next = this;

        previous = node;
    }
}

class PriorityQueueBodyNode<EntryType, ComparatorType> : PriorityQueueNode<EntryType, ComparatorType> where ComparatorType : IComparable
{
    public EntryType item { get; private set; }

    public ComparatorType priority { get; protected set; }

    public void Initialize(EntryType item, ComparatorType priority)
    {
        this.item = item;

        this.priority = priority;
    }

    public override void Bubble(PriorityQueueBodyNode<EntryType, ComparatorType> node)
    {
        if (priority.CompareTo(node.priority) > 0)
        {
            previous.next = node;

            node.previous = previous;

            node.next = this;

            previous = node;
        }
        else
        {
            next.Bubble(node);
        }
    }
}
