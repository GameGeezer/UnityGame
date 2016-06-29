using System;
using UnityEngine;

[Serializable]
public class CameraPositionSettings
{
    public Vector3 offsetFromTarget = new Vector3();
    public Vector2 zoomRange = new Vector2();
}

[Serializable]
public class CameraOrbitSettings
{
    public Vector2 rotation = new Vector2();
    public Vector2 maxRotation = new Vector2();
}
