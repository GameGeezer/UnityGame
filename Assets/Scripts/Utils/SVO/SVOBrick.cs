using System;

public class SVOBrick
{
    private byte[] data;

    private int dimX;

    private int dimY;

    private int dimZ;

    public SVOBrick(int dimX, int dimY, int dimZ)
    {
        this.dimX = dimX;

        this.dimY = dimY;

        this.dimZ = dimZ;

        data = new byte[dimX * dimY * dimZ * 2];
    }

    public void SetAt(int x, int y, int z, UInt16 value)
    {
        int index = (x + y * dimX + x * dimX * dimY) * 2;

        byte[] asByte = BitConverter.GetBytes(value);

        data[index] = asByte[0];

        data[index + 1] = asByte[1];
    }

    public UInt16 GetAt(int x, int y, int z)
    {
        int index = (x + y * dimX + x * dimX * dimY) * 2;

        return (UInt16)((data[index + 1] << 8) + data[index]); ;
    }

    public byte[] AsByteArray()
    {
        return data;
    }
}
