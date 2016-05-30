using UnityEngine;
using System.Collections;

public class Chunk
{
    private GameObject gameObject = new GameObject();

    public Mesh ChunkMesh { get; set; }

    private Pool<Chunk> pool;

    public Chunk()
    {
        ChunkMesh = new Mesh();
        gameObject.AddComponent<MeshRenderer>();
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<ChunkBehavior>();
        gameObject.GetComponent<ChunkBehavior>().chunk = this;
        gameObject.GetComponent<MeshFilter>().mesh = ChunkMesh;
    }

    public void Release()
    {
        ChunkMesh.Clear();
        pool.Release(this);
    }

    public void Initialize(Pool<Chunk> pool, Material material, float x, float y, float z)
    {
        this.pool = pool;

        gameObject.GetComponent<MeshRenderer>().material = material;
        
        gameObject.transform.Translate(x, y, z);
    }
}
