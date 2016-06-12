using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

/*
    Bricks are groups of voxels
 */
public class Brick : Grid3D<byte> {

    public Brick(int resolutionX, int resolutionY, int resolutionZ) : base((int)Math.Pow(2, resolutionX), (int)Math.Pow(2, resolutionY), (int)Math.Pow(2, resolutionZ))
    {
        
    }

    public bool RaycastNeighbor(Ray ray, int offsetX, int offsetY, int offsetZ, out Vector3i neighbor)
    {
        neighbor = GamePools.Vector3iPool.Catch();
        float closestDistance = float.MaxValue;
        int xMin, yMin, zMin;
        for (int x = 0; x < GetWidth(); ++x)
        {
            for (int z = 0; z < GetDepth(); ++z)
            {
                for (int y = 0; y < GetHeight(); ++y)
                {
                    xMin = x + offsetX;
                    yMin = y + offsetY;
                    zMin = z + offsetZ;
                    float distance;
                    if (CollisionUtil.IntersectsBounds(ray, xMin, yMin, zMin, xMin + 1, yMin + 1, zMin + 1, out distance))
                    {
                       

                        if (CollisionUtil.IntersectsPlane(ray, xMin, yMin, zMin, -1, 0, 0, out distance))
                        {
                            if (distance < closestDistance)
                            {
                                neighbor.Set(xMin - 1, yMin, zMin);
                                closestDistance = distance;
                            }
                        }
                        if (CollisionUtil.IntersectsPlane(ray, xMin, yMin, zMin, 0, -1, 0, out distance))
                        {
                            if (distance < closestDistance)
                            {
                                neighbor.Set(xMin, yMin - 1, zMin);
                                closestDistance = distance;
                            }

                        }
                        if (CollisionUtil.IntersectsPlane(ray, xMin, yMin, zMin, 0, 0, -1, out distance))
                        {
                            if (distance < closestDistance)
                            {
                                neighbor.Set(xMin, yMin, zMin - 1);
                                closestDistance = distance;
                            }

                        }
                        if (CollisionUtil.IntersectsPlane(ray, xMin + 1, yMin, zMin, 1, 0, 0, out distance))
                        {
                            if (distance < closestDistance)
                            {
                                neighbor.Set(xMin + 1, yMin, zMin);
                                closestDistance = distance;
                            }

                        }

                        if (CollisionUtil.IntersectsPlane(ray, xMin, yMin + 1, zMin, 0, 1, 0, out distance))
                        {
                            if (distance < closestDistance)
                            {
                                neighbor.Set(xMin, yMin + 1, zMin);
                                closestDistance = distance;
                            }
                        }
                        if (CollisionUtil.IntersectsPlane(ray, xMin, yMin, zMin + 1, 0, 0, 1, out distance))
                        {
                            if (distance < closestDistance)
                            {
                                neighbor.Set(xMin, yMin, zMin + 1);
                                closestDistance = distance;
                            }
                        }
                    }
                }
            }
        }

        return closestDistance == float.MaxValue;
    }

    public Vector3i RayEntersCellFromCell(Ray ray, Vector3i cell, out float outDistance)
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

    public bool RaycastAjacentCell(Ray ray, List<byte> emptySpace, float offsetX, float offsetY, float offsetZ, out Vector3i adjacent, out float distance)
    {
        PriorityQueue<Vector3i> found = new PriorityQueue<Vector3i>();

        RaycastFilledCells(ray, found, emptySpace, offsetX, offsetY, offsetZ);

        if(found.Count == 0)
        {
            adjacent = null;
            distance = float.MaxValue;
            return false;
        }

        Vector3i firstIntersection = found.Dequeue();

        ray.origin.Set(ray.origin.x - offsetX, ray.origin.y - offsetY, ray.origin.z - offsetZ);

        adjacent = RayEntersCellFromCell(ray, firstIntersection, out distance);

        ray.origin.Set(ray.origin.x + offsetX, ray.origin.y + offsetY, ray.origin.z + offsetZ);

        return true;
    }

    public void RaycastFilledCells(Ray ray, PriorityQueue<Vector3i> found, List<byte> emptySpace, float offsetX, float offsetY, float offsetZ)
    {
        ray.origin.Set(ray.origin.x - offsetX, ray.origin.y - offsetY, ray.origin.z - offsetZ);

        RaycastFilledCellsHelper(ray, found, emptySpace, 0, 0, 0, GetWidth(), GetHeight(), GetDepth());

        ray.origin.Set(ray.origin.x + offsetX, ray.origin.y + offsetY, ray.origin.z + offsetZ);
    }

    private void RaycastFilledCellsHelper(Ray ray, PriorityQueue<Vector3i> found, List<byte> emptySpace, int x, int y, int z, int width, int height, int depth)
    {
        float distance;
        if (!CollisionUtil.IntersectsBounds(ray, x, y, z, x + width, y + height, z + depth, out distance))
        {
            return;
        }

        if (width == 1 && height == 1 && depth == 1) // Work for all empty lists
        {
            if (!emptySpace.Contains(GetValue(x, y, z)))
            {
                found.Enqueue(GamePools.Vector3iPool.Catch().Set(x, y, z), 1000 - distance);
            }
        }
        else
        {
            int newWidth = (width + 1) / 2;
            int newHeight = (height + 1) / 2;
            int newDepth = (depth + 1) / 2;

            RaycastFilledCellsHelper(ray, found, emptySpace, x, y, z, newWidth, newHeight, newDepth);
            RaycastFilledCellsHelper(ray, found, emptySpace, x + newWidth, y, z, newWidth, newHeight, newDepth);
            RaycastFilledCellsHelper(ray, found, emptySpace, x, y + newHeight, z, newWidth, newHeight, newDepth);
            RaycastFilledCellsHelper(ray, found, emptySpace, x, y, z + newDepth, newWidth, newHeight, newDepth);
            RaycastFilledCellsHelper(ray, found, emptySpace, x + newWidth, y + newHeight, z, newWidth, newHeight, newDepth);
            RaycastFilledCellsHelper(ray, found, emptySpace, x + newWidth, y, z + newDepth, newWidth, newHeight, newDepth);
            RaycastFilledCellsHelper(ray, found, emptySpace, x, y + newHeight, z + newDepth, newWidth, newHeight, newDepth);
            RaycastFilledCellsHelper(ray, found, emptySpace, x + newWidth, y + newHeight, z + newDepth, newWidth, newHeight, newDepth);
        }
    }

    public void RaycastEmptyCells(Ray ray, PriorityQueue<Vector3i> found, List<byte> emptySpace, float offsetX, float offsetY, float offsetZ)
    {
        ray.origin.Set(ray.origin.x - offsetX, ray.origin.y - offsetY, ray.origin.z - offsetZ);

        RaycastEmptyCellsHelper(ray, found, emptySpace, 0, 0, 0, GetWidth(), GetHeight(), GetDepth());

        ray.origin.Set(ray.origin.x + offsetX, ray.origin.y + offsetY, ray.origin.z + offsetZ);
    }

    private void RaycastEmptyCellsHelper(Ray ray, PriorityQueue<Vector3i> found, List<byte> emptySpace, int x, int y, int z, int width, int height, int depth)
    {
        float distance;
        if (!CollisionUtil.IntersectsBounds(ray, x, y, z, x + width, y + height, z + depth, out distance))
        {
            return;
        }

        if (width == 1 && height == 1 && depth == 1) // Work for all empty lists
        {
            if(emptySpace.Contains(GetValue(x, y, z)))
            {
                found.Enqueue(GamePools.Vector3iPool.Catch().Set(x, y, z), float.MaxValue - distance);
            }  
        }
        else
        {
            int newWidth = (width + 1) / 2;
            int newHeight = (height + 1) / 2;
            int newDepth = (depth + 1) / 2;

            RaycastEmptyCellsHelper(ray, found, emptySpace, x, y, z, newWidth, newHeight, newDepth);
            RaycastEmptyCellsHelper(ray, found, emptySpace, x + newWidth, y, z, newWidth, newHeight, newDepth);
            RaycastEmptyCellsHelper(ray, found, emptySpace, x, y + newHeight, z, newWidth, newHeight, newDepth);
            RaycastEmptyCellsHelper(ray, found, emptySpace, x, y, z + newDepth, newWidth, newHeight, newDepth);
            RaycastEmptyCellsHelper(ray, found, emptySpace, x + newWidth, y + newHeight, z, newWidth, newHeight, newDepth);
            RaycastEmptyCellsHelper(ray, found, emptySpace, x + newWidth, y, z + newDepth, newWidth, newHeight, newDepth);
            RaycastEmptyCellsHelper(ray, found, emptySpace, x, y + newHeight, z + newDepth, newWidth, newHeight, newDepth);
            RaycastEmptyCellsHelper(ray, found, emptySpace, x + newWidth, y + newHeight, z + newDepth, newWidth, newHeight, newDepth);
        }
    }

    public void RaycastAllCells(Ray ray, PriorityQueue<Vector3i> found, float offsetX, float offsetY, float offsetZ)
    {
        ray.origin.Set(ray.origin.x - offsetX, ray.origin.y - offsetY, ray.origin.z - offsetZ);

        RaycastAllCellsHelper(ray, found, 0, 0, 0, GetWidth(), GetHeight(), GetDepth());

        ray.origin.Set(ray.origin.x + offsetX, ray.origin.y + offsetY, ray.origin.z + offsetZ);
    }

    private void RaycastAllCellsHelper(Ray ray, PriorityQueue<Vector3i> found, int x, int y, int z, int width, int height, int depth)
    {
        float distance;
        if (!CollisionUtil.IntersectsBounds(ray, x, y, z, x + width, y + height, z + depth, out distance))
        {
            return;
        }

        if (width == 1 && height == 1 && depth == 1) // Work for all empty lists
        {
            found.Enqueue(GamePools.Vector3iPool.Catch().Set(x, y, z), float.MaxValue - distance);
        }
        else
        {
            int newWidth = (width + 1) / 2;
            int newHeight = (height + 1) / 2;
            int newDepth = (depth + 1) / 2;

            RaycastAllCellsHelper(ray, found, x, y, z, newWidth, newHeight, newDepth);
            RaycastAllCellsHelper(ray, found, x + newWidth, y, z, newWidth, newHeight, newDepth);
            RaycastAllCellsHelper(ray, found, x, y + newHeight, z, newWidth, newHeight, newDepth);
            RaycastAllCellsHelper(ray, found, x, y, z + newDepth, newWidth, newHeight, newDepth);
            RaycastAllCellsHelper(ray, found, x + newWidth, y + newHeight, z, newWidth, newHeight, newDepth);
            RaycastAllCellsHelper(ray, found, x + newWidth, y, z + newDepth, newWidth, newHeight, newDepth);
            RaycastAllCellsHelper(ray, found, x, y + newHeight, z + newDepth, newWidth, newHeight, newDepth);
            RaycastAllCellsHelper(ray, found, x + newWidth, y + newHeight, z + newDepth, newWidth, newHeight, newDepth);
        }
    }

    public void CleanEdges()
    {
        int xEnd = GetWidth() - 1;
        int yEnd = GetHeight() - 1;
        int zEnd = GetDepth() - 1;
        for (int x = 0; x < GetWidth(); ++x)
        {
            for (int y = 0; y < GetHeight(); ++y)
            {
                SetValue(x, y, 0, 0);
                SetValue(x, y, zEnd, 0);
            }
        }

        for (int x = 0; x < GetWidth(); ++x)
        {
            for (int z = 0; z < GetDepth(); ++z)
            {
                SetValue(x, 0, z, 0);
                SetValue(x, yEnd, z, 0);
            }
        }

        for (int y = 0; y < GetHeight(); ++y)
        {
            for (int z = 0; z < GetDepth(); ++z)
            {
                SetValue(0, y, z, 0);
                SetValue(xEnd, y, z, 0);
            }
        }
    }

    public void fillWithNoise(int offsetX, int offsetY, int offsetZ, Noise2D heightNoise)
    {
        for (int x = 0; x < GetWidth(); ++x)
        {
            for (int z = 0; z < GetDepth(); ++z)
            {
                int height = heightNoise.generate(offsetX + x, offsetZ + z);

                for (int y = 0; y < GetHeight(); ++y)
                {
                    if (offsetY + y < height)
                    {
                        SetValue(x, y, z, 1);
                    }
                    else
                    {
                        SetValue(x, y, z, 0);
                    }
                }
            }
        }
    }
}
