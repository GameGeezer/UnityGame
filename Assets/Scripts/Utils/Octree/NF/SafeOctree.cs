
using UnityEngine;

public class SafeOctree<T>
{
    Octree<T> octree;

    public SafeOctree(Vector3 leafDimensions, Vector3i startPosition)
    {
        octree = new Octree<T>(leafDimensions, startPosition);
    }

    public void RayCastFind(Ray ray, PriorityQueue<float, OctreeEntry<T>> found)
    {
        lock (this)
        {
            octree.RayCastFind(ray, found);
        }
    }

    public T GetAt(Vector3i point)
    {
        lock(this)
        {
            return octree.GetAt(point);
        }
    }

    public void SetAt(Vector3i point, T value)
    {
        lock(this)
        {
            octree.SetAt(point, value);
        }
    }


    public void SetAtIfNull(Vector3i point, T value)
    {
        lock (this)
        {
            T stored = GetAt(point);

            if(stored == null)
            {
                octree.SetAt(point, value);
            }
        }
    }

    public void RemoveAt(Vector3i point)
    {
        lock(this)
        {
            octree.RemoveAt(point);
        }
    }

    public void DrawWireFrame()
    {
        octree.DrawWireFrame();
    }
}
