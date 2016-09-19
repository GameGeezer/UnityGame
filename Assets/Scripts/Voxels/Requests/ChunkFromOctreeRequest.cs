using System.Collections.Generic;
using UnityEngine;

class ChunkFromOctreeRequest : Request
{
    private Chunk chunk;
    private BrickTree brickTree;
    private CubicChunkExtractor extractor;

    private List<Color> colors = new List<Color>();
    private List<Vector3> vertices = new List<Vector3>();
    private List<Vector3> normals = new List<Vector3>();
    private List<Vector2> uv = new List<Vector2>();
    private List<int> indices = new List<int>();

    private Pool<Color> colorPool = new Pool<Color>();
    private Pool<Vector3> vector3Pool = new Pool<Vector3>();
    private Pool<Vector2> vector2Pool = new Pool<Vector2>();

    private Material material;
    private Vector3i brickCell;

    private Pool<ChunkFromOctreeRequest> parentPool;
    private Pool<Chunk> chunkPool;

    public ChunkFromOctreeRequest()
    {

    }

    public Chunk ReInitialize(BrickTree brickTree, CubicChunkExtractor extractor, Material material, int brickX, int brickY, int brickZ, Pool<Chunk> chunkPool, Pool<ChunkFromOctreeRequest> parentPool)
    {
        this.brickTree = brickTree;
        this.extractor = extractor;
        this.material = material;

        this.chunkPool = chunkPool;
        this.parentPool = parentPool;

        this.chunk = chunkPool.Catch();

        brickCell = new Vector3i(brickX, brickY, brickZ);

        return chunk;
    }

    public void PrePerformance()
    {
        vector3Pool.ReleaseAll(chunk.GetMesh().vertices);
        vector3Pool.ReleaseAll(chunk.GetMesh().normals);
        vector2Pool.ReleaseAll(chunk.GetMesh().uv);
        colors.Clear();
        vertices.Clear();
        normals.Clear();
        uv.Clear();
        indices.Clear();
    }

    public void Perform()
    {
        extractor.Extract(brickTree, brickCell, colors, vertices, normals, uv, indices, colorPool, vector2Pool, vector3Pool);
    }

    public void PostPerformance()
    {
        chunk.Clear();

        if (vertices.Count == 0)
        {
            chunkPool.Release(chunk);

            return;
        }

        chunk.Initialize(material, brickCell.x * brickTree.BrickDimensionX, brickCell.y * brickTree.BrickDimensionY, brickCell.z * brickTree.BrickDimensionZ);

        chunk.GetMesh().vertices = vertices.ToArray();
        chunk.GetMesh().colors = colors.ToArray();
        chunk.GetMesh().triangles = indices.ToArray();
        chunk.GetMesh().normals = normals.ToArray();
        chunk.GetMesh().uv = uv.ToArray(); // add this line to the code here
        chunk.GetMesh().Optimize();

        chunk.UpdateCollider(chunk.GetMesh().vertices, chunk.GetMesh().triangles);

        parentPool.Release(this);
    }
}
