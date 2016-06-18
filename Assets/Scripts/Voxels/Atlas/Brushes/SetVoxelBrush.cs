using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class SetVoxelBrush : VoxelBrush
{
    private PriorityQueue<float, Vector3i> found = new PriorityQueue<float, Vector3i>();

    private Grid3DSelectBlackList<byte> selector = new Grid3DSelectBlackList<byte>();

    public SetVoxelBrush()
    {

    }

    public override bool Stroke(Ray ray, Brick brick, Vector3 brickPosition, VoxelMaterial voxelMaterial, VoxelMaterialAtlas materialAtlas, List<byte> blackList)
    {
        found.Clear();

        selector.Select(ray, brick, brickPosition, blackList, found);

        if (found.Count == 0)
        {
            return false;
        }

        float distance = found.PeekPriority();

        Vector3i cell = found.Dequeue();

        brick.SetValue(cell.x, cell.y, cell.z, materialAtlas.GetMaterialId(voxelMaterial));

        return true;
    }
}
