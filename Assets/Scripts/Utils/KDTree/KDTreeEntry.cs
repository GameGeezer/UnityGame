
using UnityEngine;

public class KDTreeEntry<T>
{
    public Vector3 position { get; private set; }

    public T value { get; private set; }

    public void Initialize(Vector3 position, T value)
    {
        this.position = position;

        this.value = value;
    }
}
