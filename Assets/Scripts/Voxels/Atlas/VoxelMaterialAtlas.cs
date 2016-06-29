using System;
using System.Collections.Generic;
using UnityEngine;

public class VoxelMaterialAtlas
{
    public List<byte> airMaterials = new List<byte>();

    private Dictionary<byte, VoxelMaterial> idToMaterial = new Dictionary<byte, VoxelMaterial>();
    private Dictionary<VoxelMaterial, byte> materialToId = new Dictionary<VoxelMaterial, byte>();

    public void AddVoxelMaterial(byte id, VoxelMaterial material)
    {
        idToMaterial.Add(id, material);
        materialToId.Add(material, id);
    }

    public void LoadFromFile(string name)
    {
        TextAsset atlasText = Resources.Load(name) as TextAsset;

        string[] materials = atlasText.text.Split('#');

        foreach(var material in materials)
        {
            string[] materialData = material.Split('\n');

            int id = Int32.Parse(materialData[0].Split(':')[1]);
            int stateOfMatter = Int32.Parse(materialData[1].Split(':')[1]);
            float red = Int32.Parse(materialData[2].Split(':')[1]);
            float green = Int32.Parse(materialData[3].Split(':')[1]);
            float blue = Int32.Parse(materialData[4].Split(':')[1]);

            StateOfMatter matter = stateOfMatter == 0 ? StateOfMatter.GAS : StateOfMatter.SOLID;

            if(matter == StateOfMatter.GAS)
            {
                airMaterials.Add((byte)id);
            }

            this.AddVoxelMaterial((byte)id, new VoxelMaterial(new Color(red / 255f, green / 255f, blue / 255f, 1), matter));
        }
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
