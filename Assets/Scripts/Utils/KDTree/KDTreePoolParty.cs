using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class KDTreePoolParty<T>
{
    public Pool<KDBodyNodeX<T>> bodyNodeXPool = new Pool<KDBodyNodeX<T>>();

    public Pool<KDBodyNodeY<T>> bodyNodeYPool = new Pool<KDBodyNodeY<T>>();

    public Pool<KDBodyNodeZ<T>> bodyNodeZPool = new Pool<KDBodyNodeZ<T>>();

    public Pool<KDTreeLeafNode<T>> leafNodePool = new Pool<KDTreeLeafNode<T>>();

    public Pool<KDTreeEntry<T>> entryPool = new Pool<KDTreeEntry<T>>();
}
