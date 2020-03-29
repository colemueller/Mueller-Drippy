using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generate : MonoBehaviour {

    public GameObject holdPrefab;
    public GameObject platformPrefab;
    public Transform parentObj;

    private int spawnNum = 0;
    private int dot_streak = 0;
    private int platform_streak = 0;

    public void Start()
    {

    }

    public void Update()
    {
        if(Score._score >= spawnNum)
        {
            // random position for the x placement
            float randx = Random.Range(-2f, 2f);
            // random percentage for picking a hold or something else
            int platform_chance = Random.Range(0, 100);
            if (platform_chance >= 75 && platform_streak <= 2)
            {
                platform_streak += 1;
                dot_streak = 0;
                if (randx >= 0) {
                    GeneratePlatform(randx, 0f, 60f);
                }
                else
                {
                    GeneratePlatform(randx, -60f, 0f);
                }
            } 
            else 
            {
                dot_streak += 1;
                platform_streak = 0;
                GenerateHold(randx);
            }
            spawnNum = spawnNum + 4;
        }
    }

    public void GenerateHold(float rand)
    {
        GameObject clone = Instantiate(holdPrefab, new Vector3(rand, transform.position.y, 1), Quaternion.identity) as GameObject;
        clone.transform.SetParent(parentObj);
    }
    

    public void GeneratePlatform(float rand, float angle_min, float angle_max)
    {
        float rot = Random.Range(angle_min, angle_max);
        GameObject clone = Instantiate(platformPrefab, new Vector3(rand, transform.position.y, 1), Quaternion.Euler(0f, 0f, rot)) as GameObject;
        clone.transform.SetParent(parentObj);
    }

}
