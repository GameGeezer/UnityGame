
using UnityEngine;

public class CollisionUtil
{

    public static bool IntersectsBounds(Ray ray, float minX, float minY, float minZ, float maxX, float maxY, float maxZ, out float distance)
    {
        Vector3 dummyMin = new Vector3();
        dummyMin.Set(minX, minY, minZ);
        Vector3 dummyMax = new Vector3(); ;
        dummyMax.Set(maxX, maxY, maxZ);
        Bounds bounds = new Bounds();
        bounds.SetMinMax(dummyMin, dummyMax);

        bool intersects = bounds.IntersectRay(ray, out distance);


        return intersects;
    }
}