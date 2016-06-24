
using UnityEngine;
using System;

public class ColorUtil
{
    System.Random rand = new System.Random();

    public void Set(Color color, float r, float g, float b, float a)
    {
        color.r = r;
        color.g = g;
        color.b = b;
        color.a = a;
    }

    public void Set(ref Color color, Color other, float varyR, float varyG, float varyB)
    {
        color.r = other.r + GetRandomNumber(-varyR, varyR);
        color.g = other.g + GetRandomNumber(-varyG, varyG);
        color.b = other.b + GetRandomNumber(-varyB, varyB);
    }


    private float GetRandomNumber(float minimum, float maximum)
    {
        return (float) (rand.NextDouble() * (maximum - minimum) + minimum);
    }
}
