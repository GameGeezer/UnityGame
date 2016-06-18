
using UnityEngine;

public class SafeOctree<T>
{
    private Octree<T> octree;

    public SafeOctree(Vector3 leafDimensions, Vector3i startPosition)
    {
        octree = new Octree<T>(leafDimensions, startPosition);
    }

    public void RayCastFind(Ray ray, PriorityQueue<float, OctreeEntry<T>> found)
    {
        lock(octree)
        {
            octree.RayCastFind(ray, found);
        }
    }

    public OctreeEntry<T> GetAt(Vector3i point)
    {
        lock (octree)
        {
            return octree.GetAt(point);
        }
    }

    public void SetAt(Vector3i point, T value)
    {
        lock (octree)
        {
            octree.SetAt(point, value);
        }
    }

    public void RemoveAt(Vector3i point)
    {
        lock (octree)
        {
            octree.RemoveAt(point);
        }
    }
}
