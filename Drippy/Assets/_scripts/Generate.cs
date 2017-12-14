using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generate : MonoBehaviour {

    public GameObject holdPrefab;
    public Transform parentObj;

    public void GenerateHold()
    {
        GameObject clone = Instantiate(holdPrefab, new Vector3(Random.Range(-1.5f, 1.5f), transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
        clone.transform.SetParent(parentObj);
    }
}
