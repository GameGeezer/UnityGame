using UnityEngine;
using System.Collections.Generic;

public class VoxelWorld : MonoBehaviour {

    public Dictionary<Vector3i, Chunk> chunksAtIndex = new Dictionary<Vector3i, Chunk>();

    public float scale = 1.0f;
    public float magnitude = 1.0f;

    public Vector3i brickDimensions = new Vector3i(), worldDimensions = new Vector3i();

    private CubicChunkExtractor extractor;

    private BrickTree brickTree;

    private RequestCircle requestHandlers = new RequestCircle();

    public void Start()
    {
        List<byte> zeroSpaces = new List<byte>();
        zeroSpaces.Add(0);
        //extractor = new CubicChunkExtractor(zeroSpaces);
        Noise2D noise = new PerlinHeightmap(scale, magnitude, 1);
        brickTree = new BrickTree(brickDimensions.x, brickDimensions.y, brickDimensions.z, noise);

        for(int i = 0; i < 4; ++i)
        {
            requestHandlers.Add(new RequestHandler());
        }

        createAll();
    }

    public void Update()
    {
        GameObject camera = GameObject.FindGameObjectsWithTag("Player")[0];
        PriorityQueue<float, OctreeEntry<Brick>> found = new PriorityQueue<float, OctreeEntry<Brick>>();
        Ray ray = camera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        brickTree.RaycastFind(ray, found);

        PriorityQueue<float, Vector3i> foundCells = new PriorityQueue<float, Vector3i>();
        while(found.Count > 0)
        {
            OctreeEntry<Brick> brick = found.Dequeue();
            //brick.entry.RaycastAllCells(ray, foundCells, brick.bounds.min.x, brick.bounds.min.y, brick.bounds.min.z);
            while(foundCells.Count > 0)
            {
                Vector3i cell = foundCells.Dequeue();
                brick.entry.SetValue(cell.x, cell.y, cell.z, 1);
            }
            Material material = Resources.Load("Materials/TestMaterial", typeof(Material)) as Material;
           // requestHandler.QueueRequest(new ExtractChunkRequest(brickTree, chunksAtIndex, extractor, material, brick.cell.x, brick.cell.y, brick.cell.z));
        }

        requestHandlers.Update();
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
        RequestHandler handler = requestHandlers.Grab();
        handler.QueueRequest(new ExtractChunkRequest(brickTree, chunksAtIndex, extractor, material, brickX, brickY, brickZ));
    }
}
