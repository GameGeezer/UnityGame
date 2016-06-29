using UnityEngine;

public class OctreeEntry<T>
{
    public Bounds bounds = new Bounds();
    public T entry { get; set; }
    public Vector3i cell { get; private set; }

    public OctreeEntry()
    {
        cell = new Vector3i();
    }

    public void Initialize(T entry, int cellX, int cellY, int cellZ, Vector3 min, Vector3 max)
    {
        this.entry = entry;

        cell.Set(cellX, cellY, cellZ);

        bounds.SetMinMax(min, max);
    }

    public bool IntersectRay(Ray ray, out float distance)
    {
        return bounds.IntersectRay(ray, out distance);
    }

    public void DrawGizmos(Color color)
    {
        Gizmos.color = color;
        Gizmos.DrawWireCube(bounds.center, bounds.extents * 2);
    }

    public void Clean()
    {
        entry = default(T);
    }
}
