using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainBehavior : MonoBehaviour {

	// Use this for initialization
	void Start () {

        Octree<int> octree = new Octree<int>();
        octree.Place(0, 0, 1, 1);

        octree.Place(0, 0, 2, 2);
        octree.Place(0, 0, 3, 3);
        octree.Place(0, 4, 2, 4);
        octree.Place(3, 0, 2, 5);
        octree.Place(0, 2, 2, 6);

        int x = octree.GetAt(0, 2, 2);
    }
	
	// Update is called once per frame
	void Update () {

    }
}
