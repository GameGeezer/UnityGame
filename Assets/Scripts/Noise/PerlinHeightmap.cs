using UnityEngine;
using System.Collections;

public class PerlinHeightmap : Noise2D {

    private float scale, mag, exp;

    public PerlinHeightmap(float scale, float mag, float exp)
    {
        this.scale = scale;
        this.mag = mag;
        this.exp = exp;
    }

    public override int generate(int x, int y)
    {
        return (int)(Mathf.Pow((Mathf.PerlinNoise(x / scale, y / scale) * mag), exp));
    }
}
