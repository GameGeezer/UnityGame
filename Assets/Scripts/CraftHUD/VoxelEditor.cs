using System;
using System.Collections.Generic;
using UnityEngine;

public class VoxelEditor : MonoBehaviour
{
    public Vector3i brickResolution = new Vector3i();

    public Vector3i worldDimensions = new Vector3i();

    public string voxelAtlasFile;

    private BrickTree voxelTree;

    private VoxelMaterialAtlas materialAtlas = new VoxelMaterialAtlas();

    private CubicChunkExtractor extractor;

    private RequestCircle requestHandlers = new RequestCircle(4, 25);

    private Pool<ChunkFromOctreeRequest> extractRequestPool = new Pool<ChunkFromOctreeRequest>();

    private Dictionary<int, Chunk> chunkDictionary = new Dictionary<int, Chunk>();

    private Pool<Chunk> chunkPool = new Pool<Chunk>();


    VoxelMaterial currentMaterial;


    VoxelBrush setBrush, setAdjacentBrush, currentBrush;

    private bool symmetrical = false;

    public VoxelEditor()
    {

    }

    public void Start()
    {
        Noise2D noise = new FlatNoise(1);

        setBrush = new SetVoxelBrush();
        setAdjacentBrush = new SetVoxelAdjacentBrush();

        materialAtlas.LoadFromFile(voxelAtlasFile);

        voxelTree = new BrickTree(brickResolution, noise);

        extractor = new CubicChunkExtractor(materialAtlas);

        createAll();
    }

    public void Update()
    {
        requestHandlers.Update();

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentBrush = setBrush;
            currentMaterial = materialAtlas.GetVoxelMaterial(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentBrush = setAdjacentBrush;
            currentMaterial = materialAtlas.GetVoxelMaterial(1);
        }


        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentBrush = setAdjacentBrush;
            currentMaterial = materialAtlas.GetVoxelMaterial(2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            currentBrush = setAdjacentBrush;
            currentMaterial = materialAtlas.GetVoxelMaterial(3);
        }

        if (!Input.GetMouseButton(0))
        {
            return;
        }


        GameObject camera = GameObject.FindGameObjectsWithTag("Player")[0];

        Ray ray = camera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

        Queue<OctreeEntry<Brick>> changed = new Queue<OctreeEntry<Brick>>();

        Bounds bounds = new Bounds();
        bounds.SetMinMax(new Vector3(0, 0, 0), new Vector3(worldDimensions.x, worldDimensions.y, worldDimensions.z));

        if (currentBrush.Stroke(ray, voxelTree, currentMaterial, materialAtlas, materialAtlas.airMaterials, changed, bounds))
        {
            while (changed.Count > 0)
            {
                OctreeEntry<Brick> entry = changed.Dequeue();

                if(entry == null)
                {
                    continue;
                }

                Vector3i cell = new Vector3i((int)entry.bounds.min.x, (int)entry.bounds.min.y, (int)entry.bounds.min.z);

                Material material = Resources.Load("Materials/TestMaterial", typeof(Material)) as Material;

                if (chunkDictionary.ContainsKey(cell.GetHashCode()))
                {
                    chunkPool.Release(chunkDictionary[cell.GetHashCode()]);
                }

                ChunkFromOctreeRequest request = extractRequestPool.Catch();
                Chunk chunk = request.ReInitialize(voxelTree, extractor, material, entry.cell.x, entry.cell.y, entry.cell.z, chunkPool, extractRequestPool);
                requestHandlers.Grab().QueueRequest(request);

                int hash = cell.GetHashCode();
                if (chunkDictionary.ContainsKey(cell.GetHashCode()))
                {
                    chunkDictionary[cell.GetHashCode()] = chunk;
                }
                else
                {
                    chunkDictionary.Add(cell.GetHashCode(), chunk);
                }
            }
        }
    }

    public void OnDrawGizmos()
    {
        if (voxelTree != null)
        {
            voxelTree.DrawWireFrame();
        }

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
        ChunkFromOctreeRequest request = extractRequestPool.Catch();
        Chunk chunk = request.ReInitialize(voxelTree, extractor, material, brickX, brickY, brickZ, chunkPool, extractRequestPool);
        requestHandlers.Grab().QueueRequest(request);
        chunkDictionary.Add(new Vector3i(brickX * 16, brickY * 16, brickZ * 16).GetHashCode(), chunk);
    }
}
