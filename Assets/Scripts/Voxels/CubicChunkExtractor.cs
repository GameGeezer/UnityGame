using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CubicChunkExtractor {

    private List<int> emptySpace;

    List<Vector3> vertices = new List<Vector3>();
    List<Vector3> normals = new List<Vector3>();
    List<Vector2> uv = new List<Vector2>();
    List<int> indices = new List<int>();

    public CubicChunkExtractor(List<int> emptySpace)
    {
        this.emptySpace = emptySpace;
    }

    public Mesh Extract(Grid3D<int> grid)
    {
        Mesh mesh = new Mesh();
        vertices.Clear();
        normals.Clear();
        uv.Clear();
        indices.Clear();

        for (int x = 0; x < grid.GetWidth() - 1; ++x)
        {
            for (int y = 0; y < grid.GetHeight() - 1; ++y)
            {
                for (int z = 0; z < grid.GetDepth() - 1; ++z)
                {
                    if (CheckForTransitionX(grid, x, y, z))
                    {
                        AddQuadX(grid, x, y, z);
                    }

                    if (CheckForTransitionY(grid, x, y, z))
                    {
                        AddQuadY(grid,  x, y, z);
                    }

                    if (CheckForTransitionZ(grid, x, y, z))
                    {
                        AddQuadZ(grid, x, y, z);
                    }
                }
            }
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = indices.ToArray();
        mesh.normals = normals.ToArray();
        mesh.uv = uv.ToArray(); // add this line to the code here
        mesh.Optimize();

        return mesh;
    }

    private bool CheckForTransitionX(Grid3D<int> grid, int x, int y, int z)
    {
        return emptySpace.Contains(grid.GetValue(x, y, z)) != emptySpace.Contains(grid.GetValue(x + 1, y, z));
    }

    private bool CheckForTransitionY(Grid3D<int> grid, int x, int y, int z)
    {
        return emptySpace.Contains(grid.GetValue(x, y, z)) != emptySpace.Contains(grid.GetValue(x, y + 1, z));
    }

    private bool CheckForTransitionZ(Grid3D<int> grid, int x, int y, int z)
    {
        return emptySpace.Contains(grid.GetValue(x, y, z)) != emptySpace.Contains(grid.GetValue(x, y, z + 1));
    }

    private void AddQuadX(Grid3D<int> grid, int x, int y, int z)
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

        if (emptySpace.Contains(grid.GetValue(x, y, z)))
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

    private void AddQuadY(Grid3D<int> grid, int x, int y, int z)
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

        
        if(emptySpace.Contains(grid.GetValue(x, y, z)))
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

    private void AddQuadZ(Grid3D<int> grid, int x, int y, int z)
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

        if (emptySpace.Contains(grid.GetValue(x, y, z)))
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
