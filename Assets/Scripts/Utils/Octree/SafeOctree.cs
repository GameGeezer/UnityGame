
using UnityEngine;

public class SafeOctree<T>
{
    Octree<T> octree;

    public SafeOctree(Vector3 leafDimensions, Vector3i startPosition)
    {
        octree = new Octree<T>(leafDimensions, startPosition);
    }

    public void RayCastFind(Ray ray, PriorityQueue<OctreeEntry<T>, float> found)
    {
        lock (octree)
        {
            octree.RayCastFind(ray, found);
        }
    }

    public OctreeEntry<T> GetAt(Vector3i point)
    {
        lock(octree)
        {
            return octree.GetAt(point);
        }     
    }

    public void SetAt(Vector3i point, T value)
    {
        lock(octree)
        {
            octree.SetAt(point, value);
        }
    }


    public OctreeEntry<T> SetAtIfNull(Vector3i point, T value)
    {
        lock (octree)
        {
            OctreeEntry<T> stored = GetAt(point);

            if(stored == null)
            {
                stored = octree.SetAt(point, value);
            }

            return stored;
        }
    }

    public T RemoveAt(Vector3i point)
    {
        lock(octree)
        {
            return octree.RemoveAt(point);
        }
    }

    public void DrawWireFrame()
    {
        octree.DrawWireFrame();
    }
}
