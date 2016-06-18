using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExtractChunkRequest : Request {

    private Dictionary<Vector3i, Chunk> chunkDictionary;
    private BrickTree brickTree;
    private CubicChunkExtractor extractor;

    private List<Vector3> vertices = new List<Vector3>();
    private List<Vector3> normals = new List<Vector3>();
    private List<Vector2> uv = new List<Vector2>();
    private List<int> indices = new List<int>();

    private Material material;
    private Vector3i brickCell;

    public ExtractChunkRequest(BrickTree brickTree, Dictionary<Vector3i, Chunk> chunkDictionary, CubicChunkExtractor extractor, Material material, int brickX, int brickY, int brickZ)
    {
        this.chunkDictionary = chunkDictionary;
        this.brickTree = brickTree;
        this.extractor = extractor;

        this.material = material;
        brickCell = new Vector3i(brickX, brickY, brickZ);
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
      //  extractor.Extract(brickCell.x, brickCell.y, brickCell.z, brickTree, ref vertices, ref normals, ref uv, ref indices);
    }

    public void PostPerformance()
    {

        if(vertices.Count == 0)
        {
            return;
        }

        Chunk chunk = ChunkPool.Catch();
        chunk.Initialize(material, brickCell.x * brickTree.BrickDimensionX, brickCell.y * brickTree.BrickDimensionY, brickCell.z * brickTree.BrickDimensionZ);

        chunk.ChunkMesh.vertices = vertices.ToArray();
        chunk.ChunkMesh.triangles = indices.ToArray();
        chunk.ChunkMesh.normals = normals.ToArray();
        chunk.ChunkMesh.uv = uv.ToArray(); // add this line to the code here
        chunk.ChunkMesh.Optimize();

        lock (chunkDictionary)
        {
            if (chunkDictionary.ContainsKey(brickCell))
            {
                ChunkPool.Release(chunkDictionary[brickCell]);
                chunkDictionary[brickCell] = chunk;
            }
        }
    }
}
