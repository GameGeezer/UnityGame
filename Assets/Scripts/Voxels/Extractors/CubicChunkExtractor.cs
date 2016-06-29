using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CubicChunkExtractor {

    private VoxelMaterialAtlas materialAtlas;

    public CubicChunkExtractor(VoxelMaterialAtlas materialAtlas)
    {
        this.materialAtlas = materialAtlas;
    }

    public void Extract(BrickTree brickTree, Vector3i brickWorld, List<Color> colors, List<Vector3> vertices, List<Vector3> normals, List<Vector2> uv, List<int> indices, Pool<Color> colorPool, Pool<Vector2> vector2Pool, Pool<Vector3> vector3Pool)
    {
        int xOffset = brickTree.BrickDimensionX * brickWorld.x;
        int yOffset = brickTree.BrickDimensionY * brickWorld.y;
        int zOffset = brickTree.BrickDimensionZ * brickWorld.z;
        ColorUtil colorUtil = new ColorUtil();
        int normalDirection;
        for (int x = 0; x < brickTree.BrickDimensionX; ++x)
        {
            for (int y = 0; y < brickTree.BrickDimensionY; ++y)
            {
                for (int z = 0; z < brickTree.BrickDimensionZ; ++z)
                {
                    int trueX = x + xOffset;
                    int trueY = y + yOffset;
                    int trueZ = z + zOffset;

                    VoxelMaterial voxel = materialAtlas.GetVoxelMaterial(brickTree.GetVoxelAt(trueX, trueY, trueZ));
                    VoxelMaterial voxelPlusX = materialAtlas.GetVoxelMaterial(brickTree.GetVoxelAt(trueX + 1, trueY, trueZ));
                    VoxelMaterial voxelPlusY = materialAtlas.GetVoxelMaterial(brickTree.GetVoxelAt(trueX, trueY + 1, trueZ));
                    VoxelMaterial voxelPlusZ = materialAtlas.GetVoxelMaterial(brickTree.GetVoxelAt(trueX, trueY, trueZ + 1));

                    if (CheckForTransition(voxel, voxelPlusX, out normalDirection))
                    {
                        AddQuadX(voxel, x, y, z, normalDirection, colors, vertices, normals, uv, indices, colorPool, vector2Pool, vector3Pool, colorUtil);
                    }

                    if (CheckForTransition(voxel, voxelPlusY, out normalDirection))
                    {
                        AddQuadY(voxel, x, y, z, normalDirection, colors, vertices, normals, uv, indices, colorPool, vector2Pool, vector3Pool, colorUtil);
                    }

                    if (CheckForTransition(voxel, voxelPlusZ, out normalDirection))
                    {
                        AddQuadZ(voxel, x, y, z, normalDirection, colors, vertices, normals, uv, indices, colorPool, vector2Pool, vector3Pool, colorUtil);
                    }
                }
            }
        }
    }

    private bool CheckForTransition(VoxelMaterial start, VoxelMaterial end, out int normalDirection)
    {
        bool containsStart = start.stateOfMatter == StateOfMatter.GAS;
        normalDirection = Convert.ToInt32(!containsStart);
        return containsStart != (end.stateOfMatter == StateOfMatter.GAS);
    }

    private void AddQuadX(VoxelMaterial voxel, int x, int y, int z, int normalDirection, List<Color> colors, List<Vector3> vertices, List<Vector3> normals, List<Vector2> uv, List<int> indices, Pool<Color> colorPool, Pool<Vector2> vector2Pool, Pool<Vector3> vector3Pool, ColorUtil colorUtil)
    {
        int vertexIndex = vertices.Count;

        Color color = colorPool.Catch();
        color = voxel.color;
        colors.Add(color);

        color = colorPool.Catch();
        color = voxel.color;
        colors.Add(color);

        color = colorPool.Catch();
        color = voxel.color;
        colors.Add(color);

        color = colorPool.Catch();
        color = voxel.color;
        colors.Add(color);

        Vector3 fish = vector3Pool.Catch();
        fish.Set(x + 1, y, z);
        vertices.Add(fish);
        fish = vector3Pool.Catch();
        fish.Set(x + 1, y + 1, z);
        vertices.Add(fish);
        fish = vector3Pool.Catch();
        fish.Set(x + 1, y, z + 1);
        vertices.Add(fish);
        fish = vector3Pool.Catch();
        fish.Set(x + 1, y + 1, z + 1);
        vertices.Add(fish);

        fish = vector3Pool.Catch();
        fish.Set(normalDirection, 0, 0);
        normals.Add(fish);
        normals.Add(fish);
        normals.Add(fish);
        normals.Add(fish);

        Vector2 smallFish = vector2Pool.Catch();
        smallFish.Set(0, 0);
        uv.Add(smallFish);
        smallFish = vector2Pool.Catch();
        smallFish.Set(1, 0);
        uv.Add(smallFish);
        smallFish = vector2Pool.Catch();
        smallFish.Set(0, 1);
        uv.Add(smallFish);
        smallFish = vector2Pool.Catch();
        smallFish.Set(1, 1);
        uv.Add(smallFish);

        if (voxel.stateOfMatter == StateOfMatter.GAS)
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

    private void AddQuadY(VoxelMaterial voxel, int x, int y, int z, int normalDirection,  List<Color> colors,  List<Vector3> vertices,  List<Vector3> normals,  List<Vector2> uv,  List<int> indices,  Pool<Color> colorPool,  Pool<Vector2> vector2Pool,  Pool<Vector3> vector3Pool, ColorUtil colorUtil)
    {
        int vertexIndex = vertices.Count;

        Color color = colorPool.Catch();
        color = voxel.color;
        colors.Add(color);

        color = colorPool.Catch();
        color = voxel.color;
        colors.Add(color);

        color = colorPool.Catch();
        color = voxel.color;
        colors.Add(color);

        color = colorPool.Catch();
        color = voxel.color;
        colors.Add(color);

        Vector3 fish = vector3Pool.Catch();
        fish.Set(x, y + 1, z);
        vertices.Add(fish);
        fish = vector3Pool.Catch();
        fish.Set(x + 1, y + 1, z);
        vertices.Add(fish);
        fish = vector3Pool.Catch();
        fish.Set(x, y + 1, z + 1);
        vertices.Add(fish);
        fish = vector3Pool.Catch();
        fish.Set(x + 1, y + 1, z + 1);
        vertices.Add(fish);

        fish = vector3Pool.Catch();
        fish.Set(0, normalDirection, 0);
        normals.Add(fish);
        normals.Add(fish);
        normals.Add(fish);
        normals.Add(fish);

        Vector2 smallFish = vector2Pool.Catch();
        smallFish.Set(0, 0);
        uv.Add(smallFish);
        smallFish = vector2Pool.Catch();
        smallFish.Set(1, 0);
        uv.Add(smallFish);
        smallFish = vector2Pool.Catch();
        smallFish.Set(0, 1);
        uv.Add(smallFish);
        smallFish = vector2Pool.Catch();
        smallFish.Set(1, 1);
        uv.Add(smallFish);


        if (voxel.stateOfMatter == StateOfMatter.GAS)
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

    private void AddQuadZ(VoxelMaterial voxel, int x, int y, int z, int normalDirection,  List<Color> colors,  List<Vector3> vertices,  List<Vector3> normals,  List<Vector2> uv,  List<int> indices,  Pool<Color> colorPool,  Pool<Vector2> vector2Pool,  Pool<Vector3> vector3Pool, ColorUtil colorUtil)
    {
        int vertexIndex = vertices.Count;


        Color color = colorPool.Catch();
        color = voxel.color;
        colors.Add(color);

        color = colorPool.Catch();
        color = voxel.color;
        colors.Add(color);

        color = colorPool.Catch();
        color = voxel.color;
        colors.Add(color);

        color = colorPool.Catch();
        color = voxel.color;
        colors.Add(color);

        Vector3 fish = vector3Pool.Catch();
        fish.Set(x, y, z + 1);
        vertices.Add(fish);
        fish = vector3Pool.Catch();
        fish.Set(x + 1, y, z + 1);
        vertices.Add(fish);
        fish = vector3Pool.Catch();
        fish.Set(x, y + 1, z + 1);
        vertices.Add(fish);
        fish = vector3Pool.Catch();
        fish.Set(x + 1, y + 1, z + 1);
        vertices.Add(fish);

        fish = vector3Pool.Catch();
        fish.Set(0, 0, normalDirection);
        normals.Add(fish);
        normals.Add(fish);
        normals.Add(fish);
        normals.Add(fish);

        Vector2 smallFish = vector2Pool.Catch();
        smallFish.Set(0, 0);
        uv.Add(smallFish);
        smallFish = vector2Pool.Catch();
        smallFish.Set(1, 0);
        uv.Add(smallFish);
        smallFish = vector2Pool.Catch();
        smallFish.Set(0, 1);
        uv.Add(smallFish);
        smallFish = vector2Pool.Catch();
        smallFish.Set(1, 1);
        uv.Add(smallFish);

        if (voxel.stateOfMatter == StateOfMatter.GAS)
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
