using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class KDTree<T>
{
    public KDTempNode<T> TEMP_INSTANCE = new KDTempNode<T>();

    public KDTreePoolParty<T> poolParty = new KDTreePoolParty<T>();

    public List<KDTreeEntry<T>> entries = new List<KDTreeEntry<T>>();

    private KDBodyNodeX<T> root = new KDBodyNodeX<T>();

    public KDTree()
    {

    }

    public void Insert(T value, Vector3 position)
    {
        KDTreeEntry<T> entry = poolParty.entryPool.Catch();

        entry.Initialize(position, value);

        entries.Add(entry);
    }

    public void Clear()
    {
        root.Clear(poolParty);

        root = poolParty.bodyNodeXPool.Catch();
    }

    public void Refresh()
    {
        root.Initialize(0, entries.Count, entries, this);
    }

    public KDTreeEntry<T> NearestNeighbor(Vector3 position)
    {
        int index = root.NearestNeighbor(position);

        return entries[index];
    }
}

