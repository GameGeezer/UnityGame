using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainBehavior : MonoBehaviour {

    VoxelWorld world = new VoxelWorld(new Vector3i(16, 16, 16), new Vector3i(10, 2, 10), 100);
	// Use this for initialization
	void Start () {

        //world.createAll();

        Octree<int> octree = new Octree<int>();
        octree.Place(new Vector3i(2, 0, 0), 1);

        int x = octree.GetAt(new Vector3i(2, 0, 0));
    }
	
	// Update is called once per frame
	void Update () {
	    
        
	}
}
