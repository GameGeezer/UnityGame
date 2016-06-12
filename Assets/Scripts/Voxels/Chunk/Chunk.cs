using UnityEngine;
using System.Collections;

public class Chunk
{
    private GameObject gameObject = new GameObject();

    public Mesh ChunkMesh { get; set; }

    public Chunk()
    {
        ChunkMesh = new Mesh();
        gameObject.AddComponent<MeshRenderer>();
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<ChunkBehavior>();
        gameObject.GetComponent<ChunkBehavior>().chunk = this;
        gameObject.GetComponent<MeshFilter>().mesh = ChunkMesh;
    }

    public void Clear()
    {
        GamePools.Vector3Pool.ReleaseAll(ChunkMesh.vertices);
        GamePools.Vector3Pool.ReleaseAll(ChunkMesh.normals);
        GamePools.Vector2Pool.ReleaseAll(ChunkMesh.uv);
        ChunkMesh.Clear();
        gameObject.transform.Translate(-gameObject.transform.position.x, -gameObject.transform.position.y, -gameObject.transform.position.z);
    }

    public void Initialize(Material material, float x, float y, float z)
    {
        gameObject.GetComponent<MeshRenderer>().material = material;
        
        gameObject.transform.Translate(x, y, z);
    }
}
