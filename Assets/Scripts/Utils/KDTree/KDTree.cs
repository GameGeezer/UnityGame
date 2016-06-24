using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class KDTree<T>
{
    public Pool<KDBodyNodeX<T>> bodyNodeXPool = new Pool<KDBodyNodeX<T>>();

    public Pool<KDBodyNodeY<T>> bodyNodeYPool = new Pool<KDBodyNodeY<T>>();

    public Pool<KDBodyNodeZ<T>> bodyNodeZPool = new Pool<KDBodyNodeZ<T>>();

    public Pool<KDTreeLeafNode<T>> leafNodePol = new Pool<KDTreeLeafNode<T>>();

    private Pool<KDTreeEntry<T>> entryPool = new Pool<KDTreeEntry<T>>();

    public List<KDTreeEntry<T>> entries = new List<KDTreeEntry<T>>();

    private KDBodyNodeX<T> root = new KDBodyNodeX<T>();

    public KDTree()
    {

    }

    public void Insert(T value, Vector3 position)
    {
        KDTreeEntry<T> entry = entryPool.Catch();

        entry.Initialize(position, value);

        entries.Add(entry);
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

