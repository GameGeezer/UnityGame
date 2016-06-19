using System.Collections.Generic;
using UnityEngine;

public class BrickWorld : MonoBehaviour
{
    public Vector3i brickDimensions = new Vector3i();

    public float scale = 1.0f;
    public float magnitude = 1.0f;

    public Vector3i worldDimensions = new Vector3i();

    private VoxelMaterialAtlas materialAtlas = new VoxelMaterialAtlas();

    private CubicChunkExtractor extractor;

    private BrickTree brickTree;

    private RequestCircle requestHandlers = new RequestCircle(8, 100);

    private Pool<ChunkFromOctreeRequest> extractRequestPool = new Pool<ChunkFromOctreeRequest>();

    private Dictionary<int, Chunk> chunkDictionary = new Dictionary<int, Chunk>();

    private Grid3D<Brick> world;

    private List<byte> blackList = new List<byte>();

    VoxelMaterial air;
    VoxelMaterial grass;
    VoxelMaterial dirt;

    VoxelMaterial currentMaterial;


    VoxelBrush setBrush, setAdjacentBrush, currentBrush;

    public void Start()
    {

        setBrush = new SetVoxelBrush();
        setAdjacentBrush = new SetVoxelAdjacentBrush();
        air = new VoxelMaterial(new Color(1, 0, 1), StateOfMatter.GAS);
        grass = new VoxelMaterial(new Color(0, 1, 0), StateOfMatter.SOLID);
        dirt = new VoxelMaterial(new Color(1, 0.5f, 0.5f), StateOfMatter.SOLID);
        materialAtlas.AddVoxelMaterial(0, air);
        materialAtlas.AddVoxelMaterial(1, grass);
        materialAtlas.AddVoxelMaterial(2, dirt);

        blackList.Add(0);

        world = new Grid3D<Brick>(worldDimensions.x, worldDimensions.y, worldDimensions.z);

        extractor = new CubicChunkExtractor(materialAtlas);

        Noise2D noise = new PerlinHeightmap(scale, magnitude, 1);
        brickTree = new BrickTree(brickDimensions.x, brickDimensions.y, brickDimensions.z, noise);

        createAll();
    }

    public void OnDrawGizmos()
    {
        brickTree.DrawWireFrame();
    }

    public void Update()
    {

        requestHandlers.Update();

        
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentBrush = setBrush;
            currentMaterial = air;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentBrush = setAdjacentBrush;
            currentMaterial = grass;
        }


        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentBrush = setAdjacentBrush;
            currentMaterial = dirt;
        }

        if (!Input.GetMouseButtonDown(0))
        {
            return;
        }

        
        GameObject camera = GameObject.FindGameObjectsWithTag("Player")[0];
        Ray ray = camera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        PriorityQueue<float, OctreeEntry<Brick>> found = new PriorityQueue<float, OctreeEntry<Brick>>();
        lock(brickTree)
        {
            brickTree.RaycastFind(ray, found);
        }
        

        Grid3DSelectBlackList<byte> cellSelector = new Grid3DSelectBlackList<byte>();
        OctreeEntry<Brick> entryHit = null;
        bool hit = false;
        while (found.Count > 0 && !hit)
        {
            PriorityQueue<float, Vector3i> out_found = new PriorityQueue<float, Vector3i>();
            OctreeEntry<Brick> entry = found.Dequeue();
            cellSelector.Select(ray, entry.entry, entry.bounds.min, blackList, out_found);
            if(out_found.Count > 0)
            {
                entryHit = entry;
                hit = true;
            }
        }

        if(!hit)
        {
            return;
        }

        Brick brick = entryHit.entry;



        Vector3i cell = new Vector3i((int)entryHit.bounds.min.x, (int)entryHit.bounds.min.y, (int)entryHit.bounds.min.z);

        if (currentBrush.Stroke(ray, brick, entryHit.bounds.min, currentMaterial, materialAtlas, blackList))
        {

            Material material = Resources.Load("Materials/TestMaterial", typeof(Material)) as Material;
            ChunkPool.Release(chunkDictionary[cell.GetHashCode()]);
            ChunkFromOctreeRequest request = extractRequestPool.Catch();
            Chunk chunk = request.ReInitialize(brickTree, extractor, material, cell.x / 16, cell.y / 16, cell.z / 16, extractRequestPool);
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
        Chunk chunk = request.ReInitialize(brickTree, extractor, material, brickX, brickY, brickZ, extractRequestPool);
        requestHandlers.Grab().QueueRequest(request);
        chunkDictionary.Add(new Vector3i(brickX * 16, brickY * 16, brickZ * 16).GetHashCode(), chunk);
    }
}
