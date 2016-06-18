using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EditBrick : MonoBehaviour {

    public Vector3i brickDimensions = new Vector3i();

    private VoxelMaterialAtlas materialAtlas = new VoxelMaterialAtlas();

    private CubicChunkExtractor extractor;

    private Brick brick;

    private RequestCircle requestHandlers = new RequestCircle(3, 100);

    private Pool<ChunkFromBrickRequest> extractRequestPool = new Pool<ChunkFromBrickRequest>();

    Chunk chunk;

    VoxelMaterial air;
    VoxelMaterial grass;
    VoxelMaterial dirt;

    VoxelMaterial currentMaterial;


    VoxelBrush setBrush, setAdjacentBrush, currentBrush;

    int brush = 1;

    private List<byte> blackList = new List<byte>();

    public void Start()
    {
        air = new VoxelMaterial(new Color(1, 0, 1), StateOfMatter.GAS);
        grass = new VoxelMaterial(new Color(0, 1, 0), StateOfMatter.SOLID);
        dirt = new VoxelMaterial(new Color(1, 0.5f, 0.5f), StateOfMatter.SOLID);
        materialAtlas.AddVoxelMaterial(0, air);
        materialAtlas.AddVoxelMaterial(1, grass);
        materialAtlas.AddVoxelMaterial(2, dirt);


        blackList.Add(0);
        extractor = new CubicChunkExtractor(materialAtlas);

        brick = new Brick(brickDimensions.x, brickDimensions.y, brickDimensions.z);
        brick.SetValue(1, 1, 1, 1);
        Material material = Resources.Load("Materials/TestMaterial", typeof(Material)) as Material;
        ChunkFromBrickRequest request = extractRequestPool.Catch();
        chunk = request.ReInitialize(brick, extractor, material, 1, 1, 1, extractRequestPool);
        requestHandlers.Grab().QueueRequest(request);

        setBrush = new SetVoxelBrush();
        setAdjacentBrush = new SetVoxelAdjacentBrush();
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

        if (currentBrush.Stroke(ray, brick, new Vector3(brick.GetWidth(), brick.GetHeight(), brick.GetDepth()), currentMaterial, materialAtlas, blackList))
        {
            brick.CleanEdges();

            Material material = Resources.Load("Materials/TestMaterial", typeof(Material)) as Material;
            ChunkPool.Release(chunk);
            ChunkFromBrickRequest request = extractRequestPool.Catch();
            chunk = request.ReInitialize(brick, extractor, material, 1, 1, 1, extractRequestPool);
            requestHandlers.Grab().QueueRequest(request);

        }
    }
}
