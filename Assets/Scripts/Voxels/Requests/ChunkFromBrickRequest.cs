using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ChunkFromBrickRequest : Request
{
    private Brick brick;
    private CubicChunkExtractor extractor;

    private List<Vector3> vertices = new List<Vector3>(10000);
    private List<Vector3> normals = new List<Vector3>(10000);
    private List<Vector2> uv = new List<Vector2>(10000);
    private List<int> indices = new List<int>(10000);

    private Material material;
    private Vector3i brickCell;

    private Chunk chunk;

    public ChunkFromBrickRequest()
    {

    }

    public Chunk ReInitialize(Brick brick, CubicChunkExtractor extractor, Material material, int brickX, int brickY, int brickZ)
    {
        chunk = ChunkPool.Catch();

        this.brick = brick;
        this.extractor = extractor;
        this.material = material;

        brickCell = new Vector3i(brickX, brickY, brickZ);

        return chunk;
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
        extractor.Extract(brick, ref vertices, ref normals, ref uv, ref indices);
    }

    public void PostPerformance()
    {
        if (vertices.Count == 0)
        {
            return;
        }

        chunk.Initialize(material, brickCell.x * brick.GetWidth(), brickCell.y * brick.GetHeight(), brickCell.z * brick.GetDepth());

        chunk.ChunkMesh.vertices = vertices.ToArray();
        chunk.ChunkMesh.triangles = indices.ToArray();
        chunk.ChunkMesh.normals = normals.ToArray();
        chunk.ChunkMesh.uv = uv.ToArray(); // add this line to the code here
        chunk.ChunkMesh.Optimize();
    }
}