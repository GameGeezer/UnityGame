using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EditBrick : MonoBehaviour {

    private Pool<ChunkFromBrickRequest> extractRequestPool = new Pool<ChunkFromBrickRequest>();

    public Vector3i brickDimensions = new Vector3i();

    private CubicChunkExtractor extractor;

    private Brick brick;

    private RequestHandler requestHandler = new RequestHandler();

    int x = 1;

    public void Start()
    {
        List<int> zeroSpaces = new List<int>();
        zeroSpaces.Add(0);

        extractor = new CubicChunkExtractor(zeroSpaces);
        brick = new Brick(brickDimensions.x, brickDimensions.y, brickDimensions.z);
        brick.SetValue(0, 0, 0, 1);
        Material material = Resources.Load("Materials/TestMaterial", typeof(Material)) as Material;
        ChunkFromBrickRequest request = extractRequestPool.Catch();
        Chunk chunk = request.ReInitialize(brick, extractor, material, 0, 0, 0);
        requestHandler.QueueRequest(request);
        requestHandler.Update();
    }

    public void Update()
    {
        GameObject camera = GameObject.FindGameObjectsWithTag("Player")[0];
        Ray ray = camera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

        PriorityQueue<Vector3i> foundCells = new PriorityQueue<Vector3i>();
        brick.RaycastCells(ray, foundCells, 0, 0, 0);
        bool changed = foundCells.Count > 0;
        while (foundCells.Count > 0)
        {
            Vector3i cell = foundCells.Dequeue();
            brick.SetValue(cell.x, cell.y, cell.z, 1);
        }
        if(changed)
        {
            Material material = Resources.Load("Materials/TestMaterial", typeof(Material)) as Material;
            ChunkFromBrickRequest request = extractRequestPool.Catch();
            Chunk chunk = request.ReInitialize(brick, extractor, material, 0, 0, 0);
            requestHandler.QueueRequest(request);
            requestHandler.Update();
        }

    }
}
