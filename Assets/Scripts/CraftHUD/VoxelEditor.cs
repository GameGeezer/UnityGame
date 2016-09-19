using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[SelectionBase]
[ExecuteInEditMode]
public class VoxelEditor : MonoBehaviour
{
    public string voxelAtlasFile;

    public Vector3i brickResolution = new Vector3i();

    public Vector3i worldDimensions = new Vector3i();

    public Vector3i maxDimensions = new Vector3i();

    private BrickTree voxelTree;

    public VoxelMaterialAtlas materialAtlas = new VoxelMaterialAtlas();

    private CubicChunkExtractor extractor;

    private RequestCircle requestHandlers = new RequestCircle(4, 25);

    private Pool<ChunkFromOctreeRequest> extractRequestPool = new Pool<ChunkFromOctreeRequest>();

    private Dictionary<int, Chunk> chunkDictionary = new Dictionary<int, Chunk>();

    private Pool<Chunk> chunkPool = new Pool<Chunk>();


    public VoxelEditor()
    {

    }

    public void Start()
    {
        foreach (Transform child in this.gameObject.transform)
        {
            chunkPool.Release(new Chunk(child.gameObject));
        }

        Noise2D noise = new FlatNoise(2);

        materialAtlas.LoadFromFile(voxelAtlasFile);

        voxelTree = new BrickTree(brickResolution, noise);

        extractor = new CubicChunkExtractor(materialAtlas);

        createAll();


    }

    public void Update()
    {
        requestHandlers.Update();
    }

    public void PaintWithRay(Ray ray, VoxelBrush brush, VoxelMaterial material)
    {
        Queue<OctreeEntry<Brick>> changed = new Queue<OctreeEntry<Brick>>();

        Bounds bounds = new Bounds();
        bounds.SetMinMax(new Vector3(1, 1, 1), new Vector3(maxDimensions.x * voxelTree.BrickDimensionX - 1, maxDimensions.y * voxelTree.BrickDimensionY - 1, maxDimensions.z * voxelTree.BrickDimensionZ - 1));

        if (brush.Stroke(ray, voxelTree, material, materialAtlas, materialAtlas.airMaterials, changed, bounds))
        {
            while (changed.Count > 0)
            {
                OctreeEntry<Brick> entry = changed.Dequeue();

                if (entry == null)
                {
                    continue;
                }

                createBrick(entry.cell.x, entry.cell.y, entry.cell.z);
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
        Vector3i cell = new Vector3i(brickX, brickY, brickZ);

        int cellHash = cell.GetHashCode();

        if (chunkDictionary.ContainsKey(cell.GetHashCode()))
        {
            chunkPool.Release(chunkDictionary[cell.GetHashCode()]);
        }

        Material material = Resources.Load("Materials/TestMaterial", typeof(Material)) as Material;

        ChunkFromOctreeRequest request = extractRequestPool.Catch();
        Chunk chunk = request.ReInitialize(voxelTree, extractor, material, brickX, brickY, brickZ, chunkPool, extractRequestPool);
        chunk.gameObject.transform.parent = gameObject.transform;
        chunk.gameObject.hideFlags = (HideFlags)0;
        requestHandlers.Grab().QueueRequest(request);

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
