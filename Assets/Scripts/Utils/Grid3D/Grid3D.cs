
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

[DataContract]
public class Grid3D<T>
{
    [DataMember]
    public T[] data;

    [DataMember]
    public int width { get; private set; }

    [DataMember]
    public int height { get; private set; }

    [DataMember]
    public int depth { get; private set; }

    public Grid3D(int width, int height, int depth)
    {
        this.width = width;
        this.height = height;
        this.depth = depth;

        data = new T[width * height * depth];
    }

    public void fill(T value)
    {
        for(int x = 0; x < width; ++x)
        {
            for (int y = 0; y < height; ++y)
            {
                for (int z = 0; z < depth; ++z)
                {
                    SetValue(x, y, z, value);
                }
            }
        }
    }

    public T GetValue(int x, int y, int z)
    {
        return data[x + y * width + z * width * depth];
    }

    public void SetValue(int x, int y, int z, T value)
    {
        data[x + y * width + z * width * depth] = value;
    }

    public int DataSizeInBytes()
    {
        return Marshal.SizeOf(typeof(T)) * width * height * depth;
    }

    public void JSONSerialize(MemoryStream stream, DataContractSerializer serializer)
    {
        serializer.WriteObject(stream, this);
    }
}
