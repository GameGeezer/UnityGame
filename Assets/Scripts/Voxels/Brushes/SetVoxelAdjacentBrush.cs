using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class SetVoxelAdjacentBrush : VoxelBrush
{
    private PriorityQueue<Vector3i, float> found = new PriorityQueue<Vector3i, float>();

    private Grid3DSelectBlackList<byte> selector = new Grid3DSelectBlackList<byte>();

    private Vector3i dummyVector3i = new Vector3i();

    private Vector3 dummyVector3 = new Vector3();

    public override bool Stroke(Ray ray, BrickTree tree, VoxelMaterial voxelMaterial, VoxelMaterialAtlas materialAtlas, List<byte> blackList, Queue<OctreeEntry<Brick>> outChangedBricks, Bounds bounds)
    {
        // Find the brick intersected
        OctreeEntry<Brick> brickEntry = FirstBrickIntersected(ray, tree, blackList);
        // If we can't find one return
        if(brickEntry == null)
        {
            return false;
        }

        Brick brick = brickEntry.entry;

        Vector3 brickPosition = brickEntry.bounds.min;

        dummyVector3.Set(brickEntry.cell.x, brickEntry.cell.y, brickEntry.cell.z);
        // Make sure the brick is within the legal paining bounds
        if (!bounds.Contains(dummyVector3))
        {
           // return false;
        }
        // Clear the resused found queue
        found.Clear();
        // Find which cells are intersected within the grid
        selector.Select(ray, brick, brickPosition, blackList, found);

        if (found.Count == 0)
        {
            return false;
        }

        Vector3i firstIntersection = found.Dequeue();

        Ray offsetRay = new Ray(new Vector3(ray.origin.x - brickPosition.x, ray.origin.y - brickPosition.y, ray.origin.z - brickPosition.z), ray.direction);

        float distance;
        RayEntersCellFromCell(offsetRay, firstIntersection, dummyVector3i, out distance);

        Vector3i adjacentLocal = dummyVector3i;
        Vector3i adjacentWorld = adjacentLocal + brickEntry.bounds.min;

        dummyVector3.Set(adjacentWorld.x, adjacentWorld.y, adjacentWorld.z);
        if (!bounds.Contains(dummyVector3))
        {
            return false;
        }

        tree.SetVoxelAt(adjacentWorld.x, adjacentWorld.y, adjacentWorld.z, materialAtlas.GetMaterialId(voxelMaterial));

        Vector3i cellModified = new Vector3i(adjacentWorld.x / tree.BrickDimensionX, adjacentWorld.y / tree.BrickDimensionY, adjacentWorld.z / tree.BrickDimensionZ);

        OctreeEntry<Brick> modified = tree.GetAt(cellModified.x, cellModified.y, cellModified.z);

        outChangedBricks.Enqueue(modified);

        if(adjacentLocal.x == 0)
        {
            modified = tree.GetAt(cellModified.x - 1, cellModified.y, cellModified.z);

            outChangedBricks.Enqueue(modified);
        }
        if (adjacentLocal.y == 0)
        {
            modified = tree.GetAt(cellModified.x, cellModified.y - 1, cellModified.z);

            outChangedBricks.Enqueue(modified);
        }
        if (adjacentLocal.z == 0)
        {
            modified = tree.GetAt(cellModified.x, cellModified.y, cellModified.z - 1);

            outChangedBricks.Enqueue(modified);
        }

        return true;
    }
}