using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class SetVoxelAdjacentBrush : VoxelBrush
{
    private PriorityQueue<float, Vector3i> found = new PriorityQueue<float, Vector3i>();

    private OccupiedCellSelector selector = new OccupiedCellSelector();

    public SetVoxelAdjacentBrush()
    {

    }

    public override bool Stroke(Ray ray, Brick brick, VoxelMaterial voxelMaterial, VoxelMaterialAtlas materialAtlas)
    {
        found.Clear();

        selector.Select(ray, brick, materialAtlas, found);

        if (found.Count == 0)
        {
            return false;
        }

        Vector3i firstIntersection = found.Dequeue();

        ray.origin.Set(ray.origin.x - brick.worldPosition.x, ray.origin.y - brick.worldPosition.y, ray.origin.z - brick.worldPosition.z);
        float distance;
        Vector3i adjacent = RayEntersCellFromCell(ray, firstIntersection, out distance);

        ray.origin.Set(ray.origin.x + brick.worldPosition.x, ray.origin.y + brick.worldPosition.y, ray.origin.z + brick.worldPosition.z);

        brick.SetValue(adjacent.x, adjacent.y, adjacent.z, materialAtlas.GetMaterialId(voxelMaterial));

        return true;
    }
}