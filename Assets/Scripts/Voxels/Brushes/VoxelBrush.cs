using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class VoxelBrush
{
    private PriorityQueue<OctreeEntry<Brick>, float> entryPrioirityQueue = new PriorityQueue<OctreeEntry<Brick>, float>();

    private PriorityQueue<Vector3i, float> cellPriorityQueue = new PriorityQueue<Vector3i, float>();

    private Grid3DSelectBlackList<byte> blackListSelector = new Grid3DSelectBlackList<byte>();

    public abstract bool Stroke(Ray ray, BrickTree tree, VoxelMaterial voxelMaterial, VoxelMaterialAtlas materialAtlas, List<byte> blackList, Queue<OctreeEntry<Brick>> outChangedBricks);

    protected OctreeEntry<Brick> FirstBrickIntersected(Ray ray, BrickTree tree, List<byte> blackList)
    {
        entryPrioirityQueue.Clear();

        tree.RaycastFind(ray, entryPrioirityQueue);
        
        while (!entryPrioirityQueue.IsEmpty())
        {
            cellPriorityQueue.Clear();

            OctreeEntry<Brick> entry = entryPrioirityQueue.Dequeue();

            blackListSelector.Select(ray, entry.entry, entry.bounds.min, blackList, cellPriorityQueue);

            if (cellPriorityQueue.Count > 0)
            {
                return entry;
            }
        }

        return null;
    }

    protected void RayEntersCellFromCell(Ray ray, Vector3i cell, Vector3i outCell, out float outDistance)
    {
        float closestDistance = float.MaxValue;
        float distance;

        int highX = cell.x + 1;
        int highY = cell.y + 1;
        int highZ = cell.z + 1;

        if (CollisionUtil.IntersectsBounds(ray, cell.x, cell.y, cell.z, cell.x, highY, highZ, out distance))
        {
            if (distance < closestDistance)
            {
                outCell.Set(cell.x - 1, cell.y, cell.z);
                closestDistance = distance;
            }
        }
        if (CollisionUtil.IntersectsBounds(ray, cell.x, cell.y, cell.z, highX, cell.y, highZ, out distance))
        {
            if (distance < closestDistance)
            {
                outCell.Set(cell.x, cell.y - 1, cell.z);
                closestDistance = distance;
            }
        }
        if (CollisionUtil.IntersectsBounds(ray, cell.x, cell.y, cell.z, highX, highY, cell.z, out distance))
        {
            if (distance < closestDistance)
            {
                outCell.Set(cell.x, cell.y, cell.z - 1);
                closestDistance = distance;
            }
        }
        if (CollisionUtil.IntersectsBounds(ray, highX, cell.y, cell.z, highX, highY, highZ, out distance))
        {
            if (distance < closestDistance)
            {
                outCell.Set(cell.x + 1, cell.y, cell.z);
                closestDistance = distance;
            }
        }

        if (CollisionUtil.IntersectsBounds(ray, cell.x, highY, cell.z, highX, highY, highZ, out distance))
        {
            if (distance < closestDistance)
            {
                outCell.Set(cell.x, cell.y + 1, cell.z);
                closestDistance = distance;
            }
        }
        if (CollisionUtil.IntersectsBounds(ray, cell.x, cell.y, highZ, highX, highY, highZ, out distance))
        {
            if (distance < closestDistance)
            {
                outCell.Set(cell.x, cell.y, cell.z + 1);
                closestDistance = distance;
            }
        }

        outDistance = closestDistance;
    }
}
