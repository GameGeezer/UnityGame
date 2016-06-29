
public class AABBI
{
    public Vector3i low { get; private set; }
    public Vector3i high { get; private set; }

    public AABBI()
    {
        low = new Vector3i();

        high = new Vector3i();
    }

    public bool Contains(int x, int y, int z)
    {
        return x >= low.x && x <= high.x && y >= low.y && y <= high.y && z >= low.z && z <= high.z;
    }

    public void Set(int lowX, int lowY, int lowZ, int highX, int highY, int highZ)
    {
        low.Set(lowX, lowY, lowZ);

        high.Set(highX, highY, highZ);
    }

    public void SetLow(int x, int y, int z)
    {
        low.Set(x, y, z);
    }

    public void SetHigh(int x, int y, int z)
    {
        high.Set(x, y, z);
    }
}
