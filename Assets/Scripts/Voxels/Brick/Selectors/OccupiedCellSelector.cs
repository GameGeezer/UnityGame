using UnityEngine;

public class OccupiedCellSelector : BrickCellSelector
{
    public void Select(Ray ray, Brick brick, VoxelMaterialAtlas materialAtlas, PriorityQueue<float, Vector3i> out_found)
    {
        // Relocate the ray relative to (0, 0, 0) since we're assuming the brick is there
        ray.origin.Set(ray.origin.x - brick.worldPosition.x, ray.origin.y - brick.worldPosition.y, ray.origin.z - brick.worldPosition.z);

        RaycastFilledCellsHelper(ray, brick, materialAtlas, out_found, 0, 0, 0, brick.GetWidth(), brick.GetHeight(), brick.GetDepth());
        // Move the ray back to its initial position
        ray.origin.Set(ray.origin.x + brick.worldPosition.x, ray.origin.y + brick.worldPosition.y, ray.origin.z + brick.worldPosition.z);
    }

    private void RaycastFilledCellsHelper(Ray ray, Brick brick, VoxelMaterialAtlas materialAtlas, PriorityQueue<float, Vector3i> out_found, int x, int y, int z, int width, int height, int depth)
    {
        float distance;
        if (!CollisionUtil.IntersectsBounds(ray, x, y, z, x + width, y + height, z + depth, out distance))
        {
            return;
        }

        if (width == 1 && height == 1 && depth == 1) // Work for all empty lists
        {
            if (materialAtlas.GetVoxelMaterial(brick.GetValue(x, y, z)).stateOfMatter == StateOfMatter.SOLID)
            {
                out_found.Enqueue(new Vector3i(x, y, z), BrickConstants.LARGE_FLOAT - distance);
            }
        }
        else
        {
            int newWidth = (width + 1) / 2;
            int newHeight = (height + 1) / 2;
            int newDepth = (depth + 1) / 2;

            RaycastFilledCellsHelper(ray, brick, materialAtlas, out_found,  x, y, z, newWidth, newHeight, newDepth);
            RaycastFilledCellsHelper(ray, brick, materialAtlas, out_found, x + newWidth, y, z, newWidth, newHeight, newDepth);
            RaycastFilledCellsHelper(ray, brick, materialAtlas, out_found,  x, y + newHeight, z, newWidth, newHeight, newDepth);
            RaycastFilledCellsHelper(ray, brick, materialAtlas, out_found, x, y, z + newDepth, newWidth, newHeight, newDepth);
            RaycastFilledCellsHelper(ray, brick, materialAtlas, out_found, x + newWidth, y + newHeight, z, newWidth, newHeight, newDepth);
            RaycastFilledCellsHelper(ray, brick, materialAtlas, out_found, x + newWidth, y, z + newDepth, newWidth, newHeight, newDepth);
            RaycastFilledCellsHelper(ray, brick, materialAtlas, out_found, x, y + newHeight, z + newDepth, newWidth, newHeight, newDepth);
            RaycastFilledCellsHelper(ray, brick, materialAtlas, out_found, x + newWidth, y + newHeight, z + newDepth, newWidth, newHeight, newDepth);
        }
    }
}
