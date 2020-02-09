using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generate : MonoBehaviour {

    public GameObject holdPrefab;
    public Transform parentObj;

    private int spawnNum = 0;

    public void Start()
    {

        //GenerateHold();
    }

    public void Update()
    {
        if(Score._score >= spawnNum)
        {
            print("spawn");
            GenerateHold();
            spawnNum = spawnNum + 4;
        }
    }

    public void GenerateHold()
    {
        float rand = Random.Range(-2f, 2f);
        //Debug.Log(rand);
        GameObject clone = Instantiate(holdPrefab, new Vector3(rand, transform.position.y, 1), Quaternion.identity) as GameObject;
        clone.transform.SetParent(parentObj);
    }

}
