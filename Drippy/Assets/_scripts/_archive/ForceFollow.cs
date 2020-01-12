using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceFollow : MonoBehaviour {

    public GameObject Drippy;
    
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(Drippy.transform.position.x, Drippy.transform.position.y, transform.position.z);
	}
}
