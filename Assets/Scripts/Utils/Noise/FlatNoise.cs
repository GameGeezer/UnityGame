using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class FlatNoise : Noise2D
{
    private int level;

    public FlatNoise(int level)
    {
        this.level = level;
    }

    public override int generate(float x, float y)
    {
        return level;
    }
}
