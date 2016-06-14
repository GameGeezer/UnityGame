using UnityEngine;

public class VoxelMaterial
{
    public Color color { get; private set; }
    public StateOfMatter stateOfMatter { get; private set; }

    public VoxelMaterial(Color color, StateOfMatter stateOfMatter)
    {
        this.color = color;
        this.stateOfMatter = stateOfMatter;
    }
}
