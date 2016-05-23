using UnityEngine;

public abstract class SDPrimitive {

    Matrix4x4 transform;

    public SDPrimitive(Vector3 translation, Quaternion rotation, Vector3 scale)
    {
        transform = Matrix4x4.TRS(translation, rotation, scale);
    }

    public abstract float distanceFrom(Vector3 point);
}
