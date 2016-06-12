using UnityEngine;
using System.Collections.Generic;

public class VoxelWorld : MonoBehaviour {

    public Dictionary<Vector3i, Chunk> chunksAtIndex = new Dictionary<Vector3i, Chunk>();

    public float scale = 1.0f;
    public float magnitude = 1.0f;

    public Vector3i brickDimensions = new Vector3i(), worldDimensions = new Vector3i();

    private CubicChunkExtractor extractor;

    private BrickTree brickTree;

    private RequestHandler requestHandler = new RequestHandler();
    private RequestHandler requestHandler2 = new RequestHandler();
    private RequestHandler requestHandler3 = new RequestHandler();
    private RequestHandler requestHandler4 = new RequestHandler();

    int x = 1;

    public void Start()
    {
        List<byte> zeroSpaces = new List<byte>();
        zeroSpaces.Add(0);
        extractor = new CubicChunkExtractor(zeroSpaces);
        Noise2D noise = new PerlinHeightmap(scale, magnitude, 1);
        brickTree = new BrickTree(brickDimensions.x, brickDimensions.y, brickDimensions.z, noise);
        createAll();
    }

    public void Update()
    {
        GameObject camera = GameObject.FindGameObjectsWithTag("Player")[0];
        PriorityQueue<OctreeEntry<Brick>> found = new PriorityQueue<OctreeEntry<Brick>>();
        Ray ray = camera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        brickTree.RaycastFind(ray, found);

        PriorityQueue<Vector3i> foundCells = new PriorityQueue<Vector3i>();
        while(found.Count > 0)
        {
            OctreeEntry<Brick> brick = found.Dequeue();
            brick.entry.RaycastAllCells(ray, foundCells, brick.bounds.min.x, brick.bounds.min.y, brick.bounds.min.z);
            while(foundCells.Count > 0)
            {
                Vector3i cell = foundCells.Dequeue();
                brick.entry.SetValue(cell.x, cell.y, cell.z, 1);
            }
            Material material = Resources.Load("Materials/TestMaterial", typeof(Material)) as Material;
           // requestHandler.QueueRequest(new ExtractChunkRequest(brickTree, chunksAtIndex, extractor, material, brick.cell.x, brick.cell.y, brick.cell.z));
        }
        requestHandler.Update();
        requestHandler2.Update();
        requestHandler3.Update();
        requestHandler4.Update();
    }

    public void createAll()
    {
        for (int x = 0; x < worldDimensions.x; ++x)
        {
            for (int y = 0; y < worldDimensions.y; ++y)
            {
                for (int z = 0; z < worldDimensions.z; ++z)
                {
                    createBrick(x, y, z);
                }
            }
        }
    }

    public void createBrick(int brickX, int brickY, int brickZ)
    {
        Material material = Resources.Load("Materials/TestMaterial", typeof(Material)) as Material;

        if(x % 4 == 0)
        {
            requestHandler.QueueRequest(new ExtractChunkRequest(brickTree, chunksAtIndex, extractor, material, brickX, brickY, brickZ));
        }
        else if(x % 4 == 1)
        {
            requestHandler2.QueueRequest(new ExtractChunkRequest(brickTree, chunksAtIndex, extractor, material, brickX, brickY, brickZ));
        }
        else if (x % 4 == 2)
        {
            requestHandler3.QueueRequest(new ExtractChunkRequest(brickTree, chunksAtIndex, extractor, material, brickX, brickY, brickZ));
        }
        else if (x % 4 == 3)
        {
            requestHandler4.QueueRequest(new ExtractChunkRequest(brickTree, chunksAtIndex, extractor, material, brickX, brickY, brickZ));
        }

        ++x;
    }
}
