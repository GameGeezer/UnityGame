using UnityEngine;
using System.Collections;

public class OctreeLeafNode<T> : OctreeNode<T>
{
    private T[] children = new T[8];

    public OctreeLeafNode(Vector3i center) : base(center, 0)
    {

    }

    public override T GetAt(Vector3i position)
    {
        int index = (int)ChildRelativeTo(position);

        return children[index];
    }

    public override void SetAt(Vector3i position, T value)
    {
        int index = (int)ChildRelativeTo(position);

        children[index] = value;
    }
}
