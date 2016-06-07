using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExtractChunkRequest : Request {

    private BrickTree brickTree;
    private CubicChunkExtractor extractor;

    private List<Vector3> vertices = new List<Vector3>(10000);
    private List<Vector3> normals = new List<Vector3>(10000);
    private List<Vector2> uv = new List<Vector2>(10000);
    private List<int> indices = new List<int>(10000);

    private Material material;
    private int brickX, brickY, brickZ;

    public ExtractChunkRequest(BrickTree brickTree, CubicChunkExtractor extractor, Material material, int brickX, int brickY, int brickZ)
    {
        this.brickTree = brickTree;
        this.extractor = extractor;

        this.material = material;
        this.brickX = brickX;
        this.brickY = brickY;
        this.brickZ = brickZ;
    }

    public void PrePerformance()
    {
        vertices.Clear();
        normals.Clear();
        uv.Clear();
        indices.Clear();
    }

    public void Perform()
    {
        extractor.Extract(brickX, brickY, brickZ, brickTree, ref vertices, ref normals, ref uv, ref indices);
    }

    public void PostPerformance()
    {
        if(vertices.Count == 0)
        {
            return;
        }

        Chunk chunk = ChunkPool.Catch();
        chunk.Initialize(material, brickX * brickTree.BrickDimensionX, brickY * brickTree.BrickDimensionY, brickZ * brickTree.BrickDimensionZ);

        chunk.ChunkMesh.vertices = vertices.ToArray();
        chunk.ChunkMesh.triangles = indices.ToArray();
        chunk.ChunkMesh.normals = normals.ToArray();
        chunk.ChunkMesh.uv = uv.ToArray(); // add this line to the code here
        chunk.ChunkMesh.Optimize();
    }
}
