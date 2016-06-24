using System;

public class Morton64
{
    public UInt64 code { get; private set; }

    public void Encode(UInt32 x, UInt32 y, UInt32 z)
    {
        code = (SplitBy3(x) | (SplitBy3(y) << 1) | (SplitBy3(z) << 2));
    }

    public UInt64 HighestOrderBitDifferent(Morton64 other)
    {
        return HighestOrderBit(code ^ other.code);
    }

    private UInt64 SplitBy3(UInt32 value)
    {
        UInt64 x = value & 0x1fffff;

        x = (x | x << 32) & 0x1f00000000ffff;
        x = (x | x << 16) & 0x1f0000ff0000ff;
        x = (x | x << 8) & 0x100f00f00f00f00f;
        x = (x | x << 4) & 0x10c30c30c30c30c3;
        x = (x | x << 2) & 0x1249249249249249;

        return x;
    }

    private UInt64 HighestOrderBit(UInt64 n)
    {
        n |= (n >> 1);
        n |= (n >> 2);
        n |= (n >> 4);
        n |= (n >> 8);
        n |= (n >> 16);
        n |= (n >> 32);

        return n - (n >> 1);
    }
}
