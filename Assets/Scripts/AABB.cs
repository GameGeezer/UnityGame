using UnityEngine;
using System.Collections;

public class AABB {

    public Vector3 Lower { get; private set; }
    public Vector3 Upper { get; private set; }

    public AABB(Vector3 lower, Vector3 upper)
    {
        this.Lower = lower;
        this.Upper = upper;
    }
}
