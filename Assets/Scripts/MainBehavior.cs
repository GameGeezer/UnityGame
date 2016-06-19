using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainBehavior : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Bounds b = new Bounds();
            b.SetMinMax(new Vector3(0, 0, 0), new Vector3(1, 1, 1));
        bool x = b.Contains(new Vector3(0, 0, 0));
        bool y = b.Contains(new Vector3(1, 1, 1));
        bool z = b.Contains(new Vector3(1, 0, 0));
    }
	
	// Update is called once per frame
	void Update () {

    }
}
