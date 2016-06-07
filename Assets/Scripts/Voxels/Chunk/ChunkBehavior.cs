using UnityEngine;
using System.Collections;

public class ChunkBehavior : MonoBehaviour {

    private GameObject player;

    public Chunk chunk;

    public ChunkBehavior()
    {
        
    }

    void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0];
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        float magnitude = (GetComponent<MeshFilter>().transform.position - player.transform.position).magnitude;

        if(magnitude > 500)
        {
            ChunkPool.Release(chunk);
        }
    }
}
