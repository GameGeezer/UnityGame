using UnityEngine;

public class OctreeEntry<T>
{
    public Bounds bounds = new Bounds();
    public T entry { get; set; }

    public OctreeEntry()
    {

    }

    public void ReInitialize(T entry, Vector3 min, Vector3 max)
    {
        bounds.SetMinMax(min, max);
        this.entry = entry;
    }

    public void Clean()
    {
        entry = default(T);
    }
}
