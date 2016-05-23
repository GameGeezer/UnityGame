using UnityEngine;
using System.Collections.Generic;

public class VoxelWorld : MonoBehaviour {

    private int allocatedChunks;

    private Vector3i brickDimensions, worldDimensions;

    private CubicChunkExtractor extractor;

    private Stack<Brick> brickPool = new Stack<Brick>();

    private Stack<Chunk> chunkPool = new Stack<Chunk>();

    public VoxelWorld(Vector3i brickDimensions, Vector3i worldDimensions, int chunksToAllocate)
    {
        this.allocatedChunks = chunksToAllocate;
        this.brickDimensions = brickDimensions;
        this.worldDimensions = worldDimensions;

        List<int> zeroSpaces = new List<int>();
        zeroSpaces.Add(0);
        extractor = new CubicChunkExtractor(zeroSpaces);
    }

    void Update()
    {

    }

    public void createAll()
    {
        for (int x = 0; x < worldDimensions.x; ++x)
        {
            for (int y = 0; y < worldDimensions.y; ++y)
            {
                for (int z = 0; z < worldDimensions.z; ++z)
                {
                    createBrick(x * brickDimensions.x, y * brickDimensions.y, z * brickDimensions.z);
                }
            }
        }
    }

    public void createBrick(int brickX, int brickY, int brickZ)
    {
        Brick brick = new Brick(brickDimensions.x, brickDimensions.y, brickDimensions.z);
        Noise2D noise = new PerlinHeightmap(30, 16, 1);
        brick.fillWithNoise(brickX, brickY, brickZ, noise);

        Material material = Resources.Load("Materials/TestMaterial", typeof(Material)) as Material;
        GameObject gameObject = new GameObject("Wut wut");
        gameObject.AddComponent<MeshRenderer>();
        gameObject.GetComponent<MeshRenderer>().material = material;
        gameObject.AddComponent<MeshFilter>();
        gameObject.GetComponent<MeshFilter>().mesh = extractor.Extract(brick);
        gameObject.transform.Translate(brickX, brickY, brickZ);
    }
}
