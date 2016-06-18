using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class VoxelBrush
{
    public abstract bool Stroke(Ray ray, Brick brick, Vector3 brickPosition, VoxelMaterial voxelMaterial, VoxelMaterialAtlas materialAtlas, List<byte> blackList);

    protected Vector3i RayEntersCellFromCell(Ray ray, Vector3i cell, out float outDistance)
    {
        Vector3i neighbor = GamePools.Vector3iPool.Catch();
        float closestDistance = float.MaxValue;
        float distance;

        int highX = cell.x + 1;
        int highY = cell.y + 1;
        int highZ = cell.z + 1;

        if (CollisionUtil.IntersectsBounds(ray, cell.x, cell.y, cell.z, cell.x, highY, highZ, out distance))
        {
            if (distance < closestDistance)
            {
                neighbor.Set(cell.x - 1, cell.y, cell.z);
                closestDistance = distance;
            }
        }
        if (CollisionUtil.IntersectsBounds(ray, cell.x, cell.y, cell.z, highX, cell.y, highZ, out distance))
        {
            if (distance < closestDistance)
            {
                neighbor.Set(cell.x, cell.y - 1, cell.z);
                closestDistance = distance;
            }

        }
        if (CollisionUtil.IntersectsBounds(ray, cell.x, cell.y, cell.z, highX, highY, cell.z, out distance))
        {
            if (distance < closestDistance)
            {
                neighbor.Set(cell.x, cell.y, cell.z - 1);
                closestDistance = distance;
            }

        }
        if (CollisionUtil.IntersectsBounds(ray, highX, cell.y, cell.z, highX, highY, highZ, out distance))
        {
            if (distance < closestDistance)
            {
                neighbor.Set(cell.x + 1, cell.y, cell.z);
                closestDistance = distance;
            }
        }

        if (CollisionUtil.IntersectsBounds(ray, cell.x, highY, cell.z, highX, highY, highZ, out distance))
        {
            if (distance < closestDistance)
            {
                neighbor.Set(cell.x, cell.y + 1, cell.z);
                closestDistance = distance;
            }
        }
        if (CollisionUtil.IntersectsBounds(ray, cell.x, cell.y, highZ, highX, highY, highZ, out distance))
        {
            if (distance < closestDistance)
            {
                neighbor.Set(cell.x, cell.y, cell.z + 1);
                closestDistance = distance;
            }
        }

        outDistance = closestDistance;

        return neighbor;
    }
}
