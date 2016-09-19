
using System.Collections;
using System.Collections.Generic;
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

    public OctreeEntry<T> GetAt(int x, int y, int z)
    {
        lock(octree)
        {
            return octree.GetAt(x, y, z);
        }     
    }

    public void SetAt(int x, int y, int z, T value)
    {
        lock(octree)
        {
            octree.SetAt(x, y, z, value);
        }
    }


    public OctreeEntry<T> SetAtIfNull(int x, int y, int z, T value)
    {
        lock (octree)
        {
            OctreeEntry<T> stored = GetAt(x, y, z);

            if(stored == null)
            {
                stored = octree.SetAt(x, y, z, value);
            }

            return stored;
        }
    }

    public void RemoveAt(int x, int y, int z)
    {
        lock(octree)
        {
            octree.RemoveAt(x, y, z); // octree.RemoveAt(x, y, z);
        }
    }

    public List<OctreeEntry<T>> GetLeafEnumerator()
    {
        return octree.GetLeafEntries();
    }

    public void DrawWireFrame()
    {
        octree.DrawGizmos();
    }
}
