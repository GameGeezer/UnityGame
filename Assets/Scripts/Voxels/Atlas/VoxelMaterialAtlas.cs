using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class VoxelMaterialAtlas
{
    private Dictionary<byte, VoxelMaterial> idToMaterial = new Dictionary<byte, VoxelMaterial>();
    private Dictionary<VoxelMaterial, byte> materialToId = new Dictionary<VoxelMaterial, byte>();

    public void AddVoxelMaterial(byte id, VoxelMaterial material)
    {
        idToMaterial.Add(id, material);
        materialToId.Add(material, id);
    }

    public VoxelMaterial GetVoxelMaterial(byte id)
    {
        return idToMaterial[id];
    }

    public byte GetMaterialId(VoxelMaterial material)
    {
        return materialToId[material];
    }
}
