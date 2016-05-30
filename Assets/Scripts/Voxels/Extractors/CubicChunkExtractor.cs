using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CubicChunkExtractor {

    private List<int> emptySpace;

    public CubicChunkExtractor(List<int> emptySpace)
    {
        this.emptySpace = emptySpace;
    }

    public void Extract(int brickX, int brickY, int brickZ, BrickTree brickTree, ref List<Vector3> vertices, ref List<Vector3> normals, ref List<Vector2> uv, ref List<int> indices)
    {
        int xOffset = brickTree.BrickDimensionX * brickX * 16;
        int yOffset = brickTree.BrickDimensionY * brickY * 16;
        int zOffset = brickTree.BrickDimensionZ * brickZ * 16;

        BrickTreeCacheFilter cachedTree = new BrickTreeCacheFilter(brickTree);

        for (int x = 0; x < brickTree.BrickDimensionX - 1; ++x)
        {
            for (int y = 0; y < brickTree.BrickDimensionY - 1; ++y)
            {
                for (int z = 0; z < brickTree.BrickDimensionZ - 1; ++z)
                {
                    int trueX = x + xOffset;
                    int trueY = y + yOffset;
                    int trueZ = z + zOffset;

                    int voxel = cachedTree.GetVoxelAt(trueX, trueY, trueZ);
                    int voxelPlusX = cachedTree.GetVoxelAt(trueX + 1, trueY, trueZ);
                    int voxelPlusY = cachedTree.GetVoxelAt(trueX, trueY + 1, trueZ);
                    int voxelPlusZ = cachedTree.GetVoxelAt(trueX, trueY, trueZ + 1);

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

    private bool CheckForTransition(int start, int end)
    {
        return emptySpace.Contains(start) != emptySpace.Contains(end);
    }

    private void AddQuadX(int voxel, int x, int y, int z, ref List<Vector3> vertices, ref List<Vector3> normals, ref List<Vector2> uv, ref List<int> indices)
    {
        int vertexIndex = vertices.Count;

        vertices.Add(new Vector3(x + 1, y, z));
        vertices.Add(new Vector3(x + 1, y + 1, z));
        vertices.Add(new Vector3(x + 1, y, z + 1));
        vertices.Add(new Vector3(x + 1, y + 1, z + 1));

        normals.Add(new Vector3(1, 0, 0));
        normals.Add(new Vector3(1, 0, 0));
        normals.Add(new Vector3(1, 0, 0));
        normals.Add(new Vector3(1, 0, 0));

        uv.Add(new Vector2(0, 0));
        uv.Add(new Vector2(1, 0));
        uv.Add(new Vector2(0, 1));
        uv.Add(new Vector2(1, 1));

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

    private void AddQuadY(int voxel, int x, int y, int z, ref List<Vector3> vertices, ref List<Vector3> normals, ref List<Vector2> uv, ref List<int> indices)
    {
        int vertexIndex = vertices.Count;

        vertices.Add(new Vector3(x, y + 1, z));
        vertices.Add(new Vector3(x + 1, y + 1, z));
        vertices.Add(new Vector3(x, y + 1, z + 1));
        vertices.Add(new Vector3(x + 1, y + 1, z + 1));

        normals.Add(new Vector3(0, 1, 0));
        normals.Add(new Vector3(0, 1, 0));
        normals.Add(new Vector3(0, 1, 0));
        normals.Add(new Vector3(0, 1, 0));

        uv.Add(new Vector2(0, 0));
        uv.Add(new Vector2(1, 0));
        uv.Add(new Vector2(0, 1));
        uv.Add(new Vector2(1, 1));

        
        if(emptySpace.Contains(voxel))
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

    private void AddQuadZ(int voxel, int x, int y, int z, ref List<Vector3> vertices, ref List<Vector3> normals, ref List<Vector2> uv, ref List<int> indices)
    {
        int vertexIndex = vertices.Count;

        vertices.Add(new Vector3(x, y, z + 1));
        vertices.Add(new Vector3(x + 1, y, z + 1));
        vertices.Add(new Vector3(x, y + 1, z + 1));
        vertices.Add(new Vector3(x + 1, y + 1, z + 1));

        normals.Add(new Vector3(0, 0, 1));
        normals.Add(new Vector3(0, 0, 1));
        normals.Add(new Vector3(0, 0, 1));
        normals.Add(new Vector3(0, 0, 1));

        uv.Add(new Vector2(0, 0));
        uv.Add(new Vector2(1, 0));
        uv.Add(new Vector2(0, 1));
        uv.Add(new Vector2(1, 1));

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
