
using UnityEngine;

public class CollisionUtil
{
    private static Bounds bounds = new Bounds();
    private static Vector3 dummyMin = new Vector3(), dummyMax = new Vector3();

    public static bool IntersectsBounds(Ray ray, float minX, float minY, float minZ, float maxX, float maxY, float maxZ)
    {
        dummyMin.Set(minX, minY, minZ);
        dummyMax.Set(maxX, maxY, maxZ);
        bounds.SetMinMax(dummyMin, dummyMax);

        return bounds.IntersectRay(ray);
    }
}