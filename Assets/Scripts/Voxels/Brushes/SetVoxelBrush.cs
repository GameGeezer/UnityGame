using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class SetVoxelBrush : VoxelBrush
{
    private PriorityQueue<Vector3i, float> found = new PriorityQueue<Vector3i, float>();

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

        float distance = found.PeekAtPriority();

        Vector3i cell = found.Dequeue();

        brick.SetValue(cell.x, cell.y, cell.z, materialAtlas.GetMaterialId(voxelMaterial));

        return true;
    }

    public override bool Stroke(Ray ray, BrickTree tree, VoxelMaterial voxelMaterial, VoxelMaterialAtlas materialAtlas, List<byte> blackList, Queue<OctreeEntry<Brick>> outChangedBricks)
    {
        OctreeEntry<Brick> brickEntry = FirstBrickIntersected(ray, tree, blackList);

        Brick brick = brickEntry.entry;

        Vector3 brickPosition = brickEntry.bounds.min;

        found.Clear();

        selector.Select(ray, brick, brickPosition, blackList, found);

        if (found.Count == 0)
        {
            return false;
        }

        Vector3i cell = found.Dequeue();

        brick.SetValue(cell.x, cell.y, cell.z, materialAtlas.GetMaterialId(voxelMaterial));

        outChangedBricks.Enqueue(brickEntry);

        if (cell.x == 0)
        {
            OctreeEntry<Brick> modified = tree.GetAt(brickEntry.cell.x - 1, brickEntry.cell.y, brickEntry.cell.z);

            outChangedBricks.Enqueue(modified);
        }
        if (cell.y == 0)
        {
            OctreeEntry<Brick> modified = tree.GetAt(brickEntry.cell.x, brickEntry.cell.y - 1, brickEntry.cell.z);

            outChangedBricks.Enqueue(modified);
        }
        if (cell.z == 0)
        {
            OctreeEntry<Brick> modified = tree.GetAt(brickEntry.cell.x, brickEntry.cell.y, brickEntry.cell.z - 1);

            outChangedBricks.Enqueue(modified);
        }

        return true;
    }
}
