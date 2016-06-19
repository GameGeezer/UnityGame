using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainBehavior : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Vector3i c = new Vector3i(0, 0, 0);
        int ch = c.GetHashCode();
        Vector3i b = new Vector3i(1, 0, 0);
        int bh = b.GetHashCode();
    }
	
	// Update is called once per frame
	void Update () {

    }
}
