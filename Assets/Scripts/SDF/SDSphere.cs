using UnityEngine;
using System.Collections;

public class SDSphere : SDPrimitive {

    private float radius;

    public SDSphere(Vector3 translation, Quaternion rotation, Vector3 scale, float radius) : base(translation, rotation, scale)
    {
        this.radius = radius;
    }

    public override float distanceFrom(Vector3 point)
    {
        return point.magnitude - radius;
    }
}
