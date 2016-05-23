public class OctreeBodyNode<T> : OctreeNode<T>
{
    public int Level { get; private set; }

    private OctreeNode<T>[] children = new OctreeNode<T>[8];

    public OctreeBodyNode(Vector3i center, int level) : base(center, level)
    {
        this.Level = level;
    }

    public override T GetAt(Vector3i position)
    {
        int index = (int)ChildRelativeTo(position);

        if(children[index] == null)
        {
            return default(T);
        }

        return children[index].GetAt(position);
    }

    public override void SetAt(Vector3i position, T value)
    {
        int index = (int)ChildRelativeTo(position);

        if (children[index] == null)
        {
            if(Level == 1)
            {
                Vector3i nodeCenter = CenterOfChildIndex(Level, index);

                children[index] = new OctreeLeafNode<T>(nodeCenter);
            }
            else
            {
                Vector3i nodeCenter = CenterOfChildIndex(Level, index);

                children[index] = new OctreeBodyNode<T>(nodeCenter, Level - 1);
            }
        }

        children[index].SetAt(position, value);
    }

    public void SetChild(OctreeChild index, OctreeNode<T> node)
    {
        if(Level == 1 && node.GetType() == typeof(OctreeLeafNode<T>))
        {
            children[(int)index] = node;
        }
        else if(node.GetType() == typeof(OctreeBodyNode<T>))
        {
            children[(int)index] = node;
        }
        else
        {
            //TODO throw exception
        }
    }
}
