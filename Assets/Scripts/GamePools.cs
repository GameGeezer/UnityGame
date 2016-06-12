
using UnityEngine;

public class GamePools
{
    public static SafePool<Vector3i> Vector3iPool = new SafePool<Vector3i>(100);
    public static SafePool<Vector3> Vector3Pool = new SafePool<Vector3>(100);
    public static SafePool<Vector2> Vector2Pool = new SafePool<Vector2>(100);
    public static SafePool<Bounds> boundsPool = new SafePool<Bounds>(100);
}