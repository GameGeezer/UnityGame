using System.Collections.Generic;
using UnityEngine;

public class Grid3DSelectBlackList<T> : Grid3DRaycastSelector<T>
{
    public void Select(Ray ray, Grid3D<T> brick, Vector3 brickPosition, List<T> whiteList, PriorityQueue<float, Vector3i> out_found)
    {
        Ray offsetRay = new Ray(new Vector3(ray.origin.x - brickPosition.x, ray.origin.y - brickPosition.y, ray.origin.z - brickPosition.z), ray.direction);
        RaycastFilledCellsHelper(offsetRay, brick, whiteList, out_found, 0, 0, 0, brick.GetWidth(), brick.GetHeight(), brick.GetDepth());
    }

    private void RaycastFilledCellsHelper(Ray ray, Grid3D<T> brick, List<T> blackList, PriorityQueue<float, Vector3i> out_found, int x, int y, int z, int width, int height, int depth)
    {
        float distance;
        if (!CollisionUtil.IntersectsBounds(ray, x, y, z, x + width, y + height, z + depth, out distance))
        {
            return;
        }

        if (width == 1 && height == 1 && depth == 1) // Work for all empty lists
        {
            if (!blackList.Contains(brick.GetValue(x, y, z)))
            {
                out_found.Enqueue(new Vector3i(x, y, z), BrickConstants.LARGE_FLOAT - distance);
            }
        }
        else
        {
            int newWidth = (width + 1) / 2;
            int newHeight = (height + 1) / 2;
            int newDepth = (depth + 1) / 2;

            RaycastFilledCellsHelper(ray, brick, blackList, out_found, x, y, z, newWidth, newHeight, newDepth);
            RaycastFilledCellsHelper(ray, brick, blackList, out_found, x + newWidth, y, z, newWidth, newHeight, newDepth);
            RaycastFilledCellsHelper(ray, brick, blackList, out_found, x, y + newHeight, z, newWidth, newHeight, newDepth);
            RaycastFilledCellsHelper(ray, brick, blackList, out_found, x, y, z + newDepth, newWidth, newHeight, newDepth);
            RaycastFilledCellsHelper(ray, brick, blackList, out_found, x + newWidth, y + newHeight, z, newWidth, newHeight, newDepth);
            RaycastFilledCellsHelper(ray, brick, blackList, out_found, x + newWidth, y, z + newDepth, newWidth, newHeight, newDepth);
            RaycastFilledCellsHelper(ray, brick, blackList, out_found, x, y + newHeight, z + newDepth, newWidth, newHeight, newDepth);
            RaycastFilledCellsHelper(ray, brick, blackList, out_found, x + newWidth, y + newHeight, z + newDepth, newWidth, newHeight, newDepth);
        }
    }
}
