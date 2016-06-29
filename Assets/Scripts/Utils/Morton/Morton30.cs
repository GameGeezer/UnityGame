using System;

public class Morton30
{
    public static UInt32 Encode(UInt32 x, UInt32 y, UInt32 z)
    {
        return  (SplitBy3(x) | (SplitBy3(y) << 1) | (SplitBy3(z) << 2));
    }

    public static void Decode(UInt32 morton, out UInt32 x, out UInt32 y, out UInt32 z)
    {
        x = CompactBy3(morton);
        y = CompactBy3(morton >> 1);
        z = CompactBy3(morton >> 2);
    }

    public static UInt32 HighestOrderBitDifferent(UInt32 morton1, UInt32 morton2)
    {
        return HighestOrderBit(morton1 ^ morton2);
    }

    private static UInt32 SplitBy3(UInt32 value)
    {
        value &= 0x000003ff;
        value |= (value << 16);
        value &= 0x030000ff;
        value |= (value << 8);
        value &= 0x0300f00f;
        value |= (value << 4);
        value &= 0x030c30c3;
        value |= (value << 2);
        value &= 0x09249249;

        return value;
    }

    private static UInt32 CompactBy3(UInt32 value)
    {
        value &= 0x09249249;
        value |= (value >> 2);
        value &= 0x030c30c3;
        value |= (value >> 4);
        value &= 0x0300f00f;
        value |= (value >> 8);
        value &= 0x030000ff;
        value |= (value >> 16);
        value &= 0x000003ff;

        return value;
    }

    private static UInt32 HighestOrderBit(UInt32 n)
    {
        n |= (n >> 1);
        n |= (n >> 2);
        n |= (n >> 4);
        n |= (n >> 8);
        n |= (n >> 16);

        return n - (n >> 1);
    }
}
