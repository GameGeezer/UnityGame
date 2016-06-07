using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainBehavior : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Brick b = new Brick(16, 16, 16);
        b.SetValue(1, 0, 0, 1);
        PriorityQueue<Vector3i> found = new PriorityQueue<Vector3i>();
        b.RaycastCells(new Ray(new Vector3(1.5f, 0, -1), new Vector3(0, 0, 1)), found,0 , 0 ,0);

        int v = found.Count;
        Vector3i d = found.Dequeue();
    }
	
	// Update is called once per frame
	void Update () {

    }
}
