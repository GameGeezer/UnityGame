using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class BrickConstants
{
    public const int TRI_X_WEIGHT = 1, TRI_Y_WEIGHT = 2, TRiZ_WEIGHT = 4;

    public const float LARGE_FLOAT = 10000f;
}

public enum BrickTriEntry
{
    CENTER = 0,
    RIGHT = 1,
    UP = 2,
    FORWARD = 3
}

