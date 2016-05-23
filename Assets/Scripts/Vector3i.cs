using UnityEngine;
using System.Collections;

public class Vector3i {

    public int x = 0, y = 0, z = 0;

    public Vector3i(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public static Vector3i operator +(Vector3i vec1, Vector3i vec2)
    {
        return new Vector3i(vec1.x + vec2.x, vec1.y + vec2.y, vec1.z + vec2.z);
    }

    public static Vector3i operator -(Vector3i vec1, Vector3i vec2)
    {
        return new Vector3i(vec1.x - vec2.x, vec1.y - vec2.y, vec1.z - vec2.z);
    }

    public static Vector3i operator +(Vector3i vec1, int shift)
    {
        return new Vector3i(vec1.x + shift, vec1.y + shift, vec1.z + shift);
    }

    public static Vector3i operator -(Vector3i vec1, int shift)
    {
        return new Vector3i(vec1.x - shift, vec1.y - shift, vec1.z - shift);
    }
}
