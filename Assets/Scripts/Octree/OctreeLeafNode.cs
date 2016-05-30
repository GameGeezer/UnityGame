using UnityEngine;
using System.Collections;

public class OctreeLeafNode<T> : OctreeNode<T>
{
    private T[] children = new T[8];

    public OctreeLeafNode(Vector3i center) : base(center, 0)
    {

    }

    public override T GetAt(int x, int y, int z)
    {
        int index = ChildRelativeTo(x, y, z);

        return children[index];
    }

    public override void SetAt(int x, int y, int z, T value)
    {
        int index = ChildRelativeTo(x, y, z);

        children[index] = value;
    }
}
