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

        gameObject.AddComponent<MeshCollider>();
        gameObject.GetComponent<MeshCollider>().sharedMesh = ChunkMesh;
    }

    public void Clear()
    {
        ChunkMesh.Clear();
        gameObject.transform.Translate(-gameObject.transform.position.x, -gameObject.transform.position.y, -gameObject.transform.position.z);
    }

    public void Initialize(Material material, float x, float y, float z)
    {
        gameObject.GetComponent<MeshRenderer>().material = material;
        
        gameObject.transform.Translate(x, y, z);
    }

    public void UpdateCollider(Vector3[] vertices, int[] triangles)
    {
        MeshCollider collider = gameObject.GetComponent<MeshCollider>();
        collider.sharedMesh = null;
        collider.sharedMesh = ChunkMesh;
        collider.sharedMesh.vertices = vertices;
        collider.sharedMesh.triangles = triangles;
        collider.sharedMesh.RecalculateBounds();
    }
}
