
using UnityEngine;

public class CollisionUtil
{
    private static Bounds bounds = new Bounds();
    private static Plane plane = new Plane();
    private static Vector3 dummyMin = new Vector3(), dummyMax = new Vector3();

    public static bool IntersectsBounds(Ray ray, float minX, float minY, float minZ, float maxX, float maxY, float maxZ, out float distance)
    {
        Vector3 dummyMin = GamePools.Vector3Pool.Catch();
        dummyMin.Set(minX, minY, minZ);
        Vector3 dummyMax = GamePools.Vector3Pool.Catch();
        dummyMax.Set(maxX, maxY, maxZ);
        Bounds bounds = GamePools.boundsPool.Catch();
        bounds.SetMinMax(dummyMin, dummyMax);

        bool intersects = bounds.IntersectRay(ray, out distance);

        GamePools.Vector3Pool.Release(dummyMin);
        GamePools.Vector3Pool.Release(dummyMax);
        GamePools.boundsPool.Release(bounds);

        return intersects;
    }

    public static bool IntersectsPlane(Ray ray, float x, float y, float z, float nx, float ny, float nz, out float distance)
    {
        dummyMin.Set(x, y, z);
        dummyMax.Set(nx, ny, nz);
        plane.SetNormalAndPosition(dummyMax, dummyMin);
        
        return plane.Raycast(ray, out distance);
    }
}