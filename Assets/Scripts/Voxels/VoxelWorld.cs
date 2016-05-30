using UnityEngine;
using System.Collections.Generic;

public class VoxelWorld : MonoBehaviour {

    public float scale = 1.0f;
    public float magnitude = 1.0f;

    public Vector3i brickDimensions = new Vector3i(), worldDimensions = new Vector3i();

    private CubicChunkExtractor extractor;

    private Pool<Chunk> chunkPool = new Pool<Chunk>();

    private BrickTree brickTree;

    private RequestHandler requestHandler = new RequestHandler();

    public void Start()
    {
        List<int> zeroSpaces = new List<int>();
        zeroSpaces.Add(0);
        extractor = new CubicChunkExtractor(zeroSpaces);
        Noise2D noise = new PerlinHeightmap(scale, magnitude, 1);
        brickTree = new BrickTree(brickDimensions.x, brickDimensions.y, brickDimensions.z, noise);
        createAll();
    }

    public void Update()
    {
        requestHandler.Update();
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
        requestHandler.QueueRequest(new ExtractChunkRequest(chunkPool, brickTree, extractor, material, brickX, brickY, brickZ));
    }
}
