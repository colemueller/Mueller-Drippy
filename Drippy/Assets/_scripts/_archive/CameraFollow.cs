using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public GameObject Drippy;
    public float offset = 2.3f;
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(transform.position.x, Drippy.transform.position.y - offset, transform.position.z);
	}
}
