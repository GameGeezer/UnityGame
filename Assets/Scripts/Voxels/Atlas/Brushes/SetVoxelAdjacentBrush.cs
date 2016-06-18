using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class SetVoxelAdjacentBrush : VoxelBrush
{
    private PriorityQueue<float, Vector3i> found = new PriorityQueue<float, Vector3i>();

    private Grid3DSelectBlackList<byte> selector = new Grid3DSelectBlackList<byte>();

    public override bool Stroke(Ray ray, Brick brick, Vector3 brickPosition, VoxelMaterial voxelMaterial, VoxelMaterialAtlas materialAtlas, List<byte> blackList)
    {
        found.Clear();
        
        selector.Select(ray, brick, brickPosition, blackList, found);

        if (found.Count == 0)
        {
            return false;
        }

        Vector3i firstIntersection = found.Dequeue();
        Ray offsetRay = new Ray(new Vector3(ray.origin.x - brickPosition.x, ray.origin.y - brickPosition.y, ray.origin.z - brickPosition.z), ray.direction);
        float distance;
        Vector3i adjacent = RayEntersCellFromCell(offsetRay, firstIntersection, out distance);

        brick.SetValue(adjacent.x, adjacent.y, adjacent.z, materialAtlas.GetMaterialId(voxelMaterial));

        return true;
    }
}