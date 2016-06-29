using UnityEngine;
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;

struct Triangle
{
    public Vector3 position1, position2, position3;
};

public class MainBehavior : MonoBehaviour {

	// Use this for initialization
	void Start () {

        int a = new Vector3i(0, 0, 0).GetHashCode();
        int b = new Vector3i(-1, 0, 0).GetHashCode();
        int c = new Vector3i(0, -1, 0).GetHashCode();
        int d = new Vector3i(0, 0, -1).GetHashCode();
        /*
        Grid3D<byte> grid = new Grid3D<byte>(10, 10, 10);
        int size = grid.DataSizeInBytes();
        ComputeShader shader = (ComputeShader)Resources.Load("ExtractCubicMesh");
        int handle = shader.FindKernel("CSMain");
        Triangle[] triangles = new Triangle[6];
        ComputeBuffer buffer = new ComputeBuffer(8 * 72, 4 * 3 * 3);
        shader.SetBuffer(handle, "triangles", buffer);
        
        shader.Dispatch(handle, 8, 1, 1);
        buffer.GetData(triangles);

        GameObject gameObject = new GameObject();
        Mesh ChunkMesh = new Mesh();
        List<Vector3> vertices = new List<Vector3>();
        vertices.Add(triangles[0].position1);
        vertices.Add(triangles[0].position2);
        vertices.Add(triangles[0].position3);
        ChunkMesh.vertices = vertices.ToArray();
        gameObject.AddComponent<MeshRenderer>();
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<ChunkBehavior>();


        gameObject.GetComponent<MeshFilter>().mesh = ChunkMesh;



        buffer.Dispose();
        */
    }
	
	// Update is called once per frame
	void Update () {

    }
}
