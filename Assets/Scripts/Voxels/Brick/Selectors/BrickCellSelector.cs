using UnityEngine;

interface BrickCellSelector
{
    void Select(Ray ray, Brick brick, VoxelMaterialAtlas materialAtlas, PriorityQueue<float, Vector3i> out_found);
}
