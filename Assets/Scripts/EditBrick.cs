using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EditBrick : MonoBehaviour {

    private Pool<ChunkFromBrickRequest> extractRequestPool = new Pool<ChunkFromBrickRequest>();

    public Vector3i brickDimensions = new Vector3i();

    private CubicChunkExtractor extractor;

    private Brick brick;

    private RequestHandler requestHandler = new RequestHandler();

    private List<byte> zeroSpaces = new List<byte>();

    Chunk chunk;


    int x = 1;

    public void Start()
    {
        zeroSpaces.Add(0);

        extractor = new CubicChunkExtractor(zeroSpaces);
        brick = new Brick(brickDimensions.x, brickDimensions.y, brickDimensions.z);
        brick.SetValue(1, 1, 1, 1);
        Material material = Resources.Load("Materials/TestMaterial", typeof(Material)) as Material;
        ChunkFromBrickRequest request = extractRequestPool.Catch();
        chunk = request.ReInitialize(brick, extractor, material, 0, 0, 0);
        requestHandler.QueueRequest(request);
        requestHandler.Update();
    }

    public void Update()
    {
        requestHandler.Update();

        if (!Input.GetMouseButtonDown(0))
        {
            return;
        }
            
        GameObject camera = GameObject.FindGameObjectsWithTag("Player")[0];
        Ray ray = camera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

        Vector3i adjacent;
        float distance;
        if (brick.RaycastAjacentCell(ray, zeroSpaces, 0, 0, 0, out adjacent, out distance))
        {
            brick.SetValue(adjacent.x, adjacent.y, adjacent.z, 1);
            brick.CleanEdges();

            Material material = Resources.Load("Materials/TestMaterial", typeof(Material)) as Material;
            ChunkPool.Release(chunk);
            ChunkFromBrickRequest request = extractRequestPool.Catch();
            chunk = request.ReInitialize(brick, extractor, material, 0, 0, 0);
            requestHandler.QueueRequest(request);
            
        }

    }
}
