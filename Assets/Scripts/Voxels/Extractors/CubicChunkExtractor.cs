﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CubicChunkExtractor {

    private List<byte> emptySpace;

    public CubicChunkExtractor(List<byte> emptySpace)
    {
        this.emptySpace = emptySpace;
    }

    public void Extract(int brickX, int brickY, int brickZ, BrickTree brickTree, ref List<Vector3> vertices, ref List<Vector3> normals, ref List<Vector2> uv, ref List<int> indices)
    {
        int xOffset = brickTree.BrickDimensionX * brickX;
        int yOffset = brickTree.BrickDimensionY * brickY;
        int zOffset = brickTree.BrickDimensionZ * brickZ;

        BrickTreeCacheFilter cachedTree = new BrickTreeCacheFilter(brickTree);

        for (int x = 0; x < brickTree.BrickDimensionX; ++x)
        {
            for (int y = 0; y < brickTree.BrickDimensionY; ++y)
            {
                for (int z = 0; z < brickTree.BrickDimensionZ; ++z)
                {
                    int trueX = x + xOffset;
                    int trueY = y + yOffset;
                    int trueZ = z + zOffset;

                    byte voxel = cachedTree.GetVoxelAt(trueX, trueY, trueZ);
                    byte voxelPlusX = cachedTree.GetVoxelAt(trueX + 1, trueY, trueZ);
                    byte voxelPlusY = cachedTree.GetVoxelAt(trueX, trueY + 1, trueZ);
                    byte voxelPlusZ = cachedTree.GetVoxelAt(trueX, trueY, trueZ + 1);

                    if (CheckForTransition(voxel, voxelPlusX))
                    {
                        AddQuadX(voxel, x, y, z, ref vertices, ref normals, ref uv, ref indices);
                    }

                    if (CheckForTransition(voxel, voxelPlusY))
                    {
                        AddQuadY(voxel,  x, y, z, ref vertices, ref normals, ref uv, ref indices);
                    }

                    if (CheckForTransition(voxel, voxelPlusZ))
                    {
                        AddQuadZ(voxel, x, y, z, ref vertices, ref normals, ref uv, ref indices);
                    }
                }
            }
        }
        cachedTree.Clear();
    }

    public void Extract(Brick brick, ref List<Vector3> vertices, ref List<Vector3> normals, ref List<Vector2> uv, ref List<int> indices)
    {
        for (int x = 0; x < brick.GetWidth() - 1; ++x)
        {
            for (int y = 0; y < brick.GetHeight() - 1; ++y)
            {
                for (int z = 0; z < brick.GetDepth() - 1; ++z)
                {

                    byte voxel = brick.GetValue(x, y, z);
                    byte voxelPlusX = brick.GetValue(x + 1, y, z);
                    byte voxelPlusY = brick.GetValue(x, y + 1, z);
                    byte voxelPlusZ = brick.GetValue(x, y, z + 1);

                    if (CheckForTransition(voxel, voxelPlusX))
                    {
                        AddQuadX(voxel, x, y, z, ref vertices, ref normals, ref uv, ref indices);
                    }

                    if (CheckForTransition(voxel, voxelPlusY))
                    {
                        AddQuadY(voxel, x, y, z, ref vertices, ref normals, ref uv, ref indices);
                    }

                    if (CheckForTransition(voxel, voxelPlusZ))
                    {
                        AddQuadZ(voxel, x, y, z, ref vertices, ref normals, ref uv, ref indices);
                    }
                }
            }
        }
    }

    private bool CheckForTransition(byte start, byte end)
    {
        return emptySpace.Contains(start) != emptySpace.Contains(end);
    }

    private void AddQuadX(byte voxel, int x, int y, int z, ref List<Vector3> vertices, ref List<Vector3> normals, ref List<Vector2> uv, ref List<int> indices)
    {
        int vertexIndex = vertices.Count;

        Vector3 fish = GamePools.Vector3Pool.Catch();
        fish.Set(x + 1, y, z);
        vertices.Add(fish);
        fish = GamePools.Vector3Pool.Catch();
        fish.Set(x + 1, y + 1, z);
        vertices.Add(fish);
        fish = GamePools.Vector3Pool.Catch();
        fish.Set(x + 1, y, z + 1);
        vertices.Add(fish);
        fish = GamePools.Vector3Pool.Catch();
        fish.Set(x + 1, y + 1, z + 1);
        vertices.Add(fish);

        fish = GamePools.Vector3Pool.Catch();
        fish.Set(1, 0, 0);
        normals.Add(fish);
        normals.Add(fish);
        normals.Add(fish);
        normals.Add(fish);

        Vector2 smallFish = GamePools.Vector2Pool.Catch();
        smallFish.Set(0, 0);
        uv.Add(smallFish);
        smallFish = GamePools.Vector2Pool.Catch();
        smallFish.Set(1, 0);
        uv.Add(smallFish);
        smallFish = GamePools.Vector2Pool.Catch();
        smallFish.Set(0, 1);
        uv.Add(smallFish);
        smallFish = GamePools.Vector2Pool.Catch();
        smallFish.Set(1, 1);
        uv.Add(smallFish);

        if (emptySpace.Contains(voxel))
        {
            indices.Add(vertexIndex + 2);
            indices.Add(vertexIndex + 1);
            indices.Add(vertexIndex);
            

            indices.Add(vertexIndex + 1);
            indices.Add(vertexIndex + 2);
            indices.Add(vertexIndex + 3);
        }
        else
        {
            indices.Add(vertexIndex);
            indices.Add(vertexIndex + 1);
            indices.Add(vertexIndex + 2);

            indices.Add(vertexIndex + 3);
            indices.Add(vertexIndex + 2);
            indices.Add(vertexIndex + 1);
        }
    }

    private void AddQuadY(byte voxel, int x, int y, int z, ref List<Vector3> vertices, ref List<Vector3> normals, ref List<Vector2> uv, ref List<int> indices)
    {
        int vertexIndex = vertices.Count;

        Vector3 fish = GamePools.Vector3Pool.Catch();
        fish.Set(x, y + 1, z);
        vertices.Add(fish);
        fish = GamePools.Vector3Pool.Catch();
        fish.Set(x + 1, y + 1, z);
        vertices.Add(fish);
        fish = GamePools.Vector3Pool.Catch();
        fish.Set(x, y + 1, z + 1);
        vertices.Add(fish);
        fish = GamePools.Vector3Pool.Catch();
        fish.Set(x + 1, y + 1, z + 1);
        vertices.Add(fish);

        fish = GamePools.Vector3Pool.Catch();
        fish.Set(0, 1, 0);
        normals.Add(fish);
        normals.Add(fish);
        normals.Add(fish);
        normals.Add(fish);

        Vector2 smallFish = GamePools.Vector2Pool.Catch();
        smallFish.Set(0, 0);
        uv.Add(smallFish);
        smallFish = GamePools.Vector2Pool.Catch();
        smallFish.Set(1, 0);
        uv.Add(smallFish);
        smallFish = GamePools.Vector2Pool.Catch();
        smallFish.Set(0, 1);
        uv.Add(smallFish);
        smallFish = GamePools.Vector2Pool.Catch();
        smallFish.Set(1, 1);
        uv.Add(smallFish);


        if (emptySpace.Contains(voxel))
        { 
            indices.Add(vertexIndex + 2);
            indices.Add(vertexIndex);
            indices.Add(vertexIndex + 1);

            indices.Add(vertexIndex + 2);
            indices.Add(vertexIndex + 1);
            indices.Add(vertexIndex + 3);
        }
        else
        {
            indices.Add(vertexIndex + 2);
            indices.Add(vertexIndex + 1);
            indices.Add(vertexIndex);
            

            indices.Add(vertexIndex + 1);
            indices.Add(vertexIndex + 2);
            indices.Add(vertexIndex + 3);
        }
    }

    private void AddQuadZ(byte voxel, int x, int y, int z, ref List<Vector3> vertices, ref List<Vector3> normals, ref List<Vector2> uv, ref List<int> indices)
    {
        int vertexIndex = vertices.Count;

        Vector3 fish = GamePools.Vector3Pool.Catch();
        fish.Set(x, y, z + 1);
        vertices.Add(fish);
        fish = GamePools.Vector3Pool.Catch();
        fish.Set(x + 1, y, z + 1);
        vertices.Add(fish);
        fish = GamePools.Vector3Pool.Catch();
        fish.Set(x, y + 1, z + 1);
        vertices.Add(fish);
        fish = GamePools.Vector3Pool.Catch();
        fish.Set(x + 1, y + 1, z + 1);
        vertices.Add(fish);

        fish = GamePools.Vector3Pool.Catch();
        fish.Set(0, 0, 1);
        normals.Add(fish);
        normals.Add(fish);
        normals.Add(fish);
        normals.Add(fish);

        Vector2 smallFish = GamePools.Vector2Pool.Catch();
        smallFish.Set(0, 0);
        uv.Add(smallFish);
        smallFish = GamePools.Vector2Pool.Catch();
        smallFish.Set(1, 0);
        uv.Add(smallFish);
        smallFish = GamePools.Vector2Pool.Catch();
        smallFish.Set(0, 1);
        uv.Add(smallFish);
        smallFish = GamePools.Vector2Pool.Catch();
        smallFish.Set(1, 1);
        uv.Add(smallFish);

        if (emptySpace.Contains(voxel))
        {
            indices.Add(vertexIndex + 2);
            indices.Add(vertexIndex + 1);
            indices.Add(vertexIndex);


            indices.Add(vertexIndex + 1);
            indices.Add(vertexIndex + 2);
            indices.Add(vertexIndex + 3);     
        }
        else
        {
            indices.Add(vertexIndex + 2);
            indices.Add(vertexIndex);
            indices.Add(vertexIndex + 1);

            indices.Add(vertexIndex + 2);
            indices.Add(vertexIndex + 1);
            indices.Add(vertexIndex + 3);
        }
    }
}
