
using UnityEngine;

public class GamePools
{
    public static SafePool<Vector3i> Vector3iPool = new SafePool<Vector3i>(100);
    public static SafePool<Vector3> Vector3Pool = new SafePool<Vector3>(100);
}