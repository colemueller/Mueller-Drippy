using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generate : MonoBehaviour {

    public GameObject holdPrefab;
    public GameObject platformPrefab;
    public Transform parentObj;

    private int spawnNum = 5;

    public void Start()
    {
        spawnNum = 5;
    }

    public void Update()
    {
        if(Score._score >= spawnNum)
        {
            //print(Score._score +" : " + spawnNum);
            // random position for the x placement
            float randx = Random.Range(-2f, 2f);
            // random percentage for picking a hold or something else
            int platform = Random.Range(0, 100);
            if (platform <= 75)
            {
                GenerateHold(randx);
            } 
            else 
            {
                GeneratePlatform(randx);
            }
            spawnNum = spawnNum + 4;
        }
    }

    public void GenerateHold(float rand)
    {
        GameObject clone = Instantiate(holdPrefab, new Vector3(rand, transform.position.y, 1), Quaternion.identity) as GameObject;
        clone.transform.SetParent(parentObj);
    }
    

    public void GeneratePlatform(float rand)
    {
        float rot = Random.Range(-60f, 60f);
        GameObject clone = Instantiate(platformPrefab, new Vector3(rand, transform.position.y, 1), Quaternion.Euler(0f, 0f, rot)) as GameObject;
        clone.transform.SetParent(parentObj);
    }

}
