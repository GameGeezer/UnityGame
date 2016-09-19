using UnityEngine;
using System.Collections;

public class Chunk
{
    public GameObject gameObject;

    public Chunk()
    {
        gameObject = new GameObject();
        gameObject.AddComponent<MeshRenderer>();
        gameObject.GetComponent<MeshRenderer>().receiveShadows = true;
        gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;

        gameObject.AddComponent<MeshFilter>();
        gameObject.GetComponent<MeshFilter>().sharedMesh = new Mesh();


        gameObject.AddComponent<ChunkBehavior>();
        gameObject.GetComponent<ChunkBehavior>().chunk = this;

        gameObject.AddComponent<MeshCollider>();
        gameObject.GetComponent<MeshCollider>().sharedMesh = gameObject.GetComponent<MeshFilter>().sharedMesh;
    }

    public Chunk(GameObject gameObject)
    {
        this.gameObject = gameObject;
    }

    public void Clear()
    {
        gameObject.GetComponent<MeshFilter>().sharedMesh.Clear();

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
        collider.sharedMesh = gameObject.GetComponent<MeshFilter>().sharedMesh;
        collider.sharedMesh.vertices = vertices;
        collider.sharedMesh.triangles = triangles;
        collider.sharedMesh.RecalculateBounds();
    }

    public Mesh GetMesh()
    {
        return gameObject.GetComponent<MeshFilter>().sharedMesh;
    }
}
