using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generate : MonoBehaviour {

    public GameObject holdPrefab;
    public GameObject movingHoldPrefab;
    public GameObject platformPrefab;
    public Transform parentObj;
    public SpriteRenderer bg_sprite;
    public PhysicsMaterial2D friction_mtl_0;
    public PhysicsMaterial2D friction_mtl_2;
    public PhysicsMaterial2D friction_mtl_4;

    private int spawnNum = 5;
    private int dot_streak = 0;
    private int platform_streak = 0;

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
            int platform_chance = Random.Range(0, 100);

            if (platform_chance >= 75 && platform_streak <= 2)
            {
                platform_streak += 1;
                dot_streak = 0;
                if (randx >= 0) {
                    GeneratePlatform(randx, 0f, 50f);
                }
                else
                {
                    GeneratePlatform(randx, -50f, 0f);
                }
            } 
            else 
            {
                dot_streak += 1;
                platform_streak = 0;
                if (Random.Range(0, 50) <= 20)
                {
                    GenerateHold(randx);
                }
                else
                {
                    GenerateMovingHold(randx);
                }
            }

            spawnNum = spawnNum + 4;
        }
    }

    public void GenerateHold(float rand)
    {
        GameObject clone = Instantiate(holdPrefab, new Vector3(rand, transform.position.y, 2), Quaternion.identity) as GameObject;
        clone.transform.SetParent(parentObj);
    }

     public void GenerateMovingHold(float rand)
    {
        GameObject clone = Instantiate(movingHoldPrefab, new Vector3(rand, transform.position.y, 2), Quaternion.identity) as GameObject;
        clone.transform.SetParent(parentObj);
    }
    

    public void GeneratePlatform(float rand, float angle_min, float angle_max)
    {
        float rot = Random.Range(angle_min, angle_max);
        GameObject clone = Instantiate(platformPrefab, new Vector3(rand, transform.position.y, 2), Quaternion.Euler(0f, 0f, rot)) as GameObject;
        clone.GetComponent<BoxCollider2D>().sharedMaterial = friction_mtl_2;
        clone.GetComponent<Rigidbody2D>().sharedMaterial = friction_mtl_2;
        // if (Mathf.Abs(rot) <= 15) 
        // {
        //     clone.GetComponent<BoxCollider2D>().sharedMaterial = friction_mtl_0;
        //     clone.GetComponent<Rigidbody2D>().sharedMaterial = friction_mtl_0;
        // }
        // else if (Mathf.Abs(rot) > 15 && Mathf.Abs(rot) <= 25)
        // {
        //     clone.GetComponent<BoxCollider2D>().sharedMaterial = friction_mtl_2;
        //     clone.GetComponent<Rigidbody2D>().sharedMaterial = friction_mtl_2;
        // }
        // else
        // {
        //     clone.GetComponent<BoxCollider2D>().sharedMaterial = friction_mtl_4;
        //     clone.GetComponent<Rigidbody2D>().sharedMaterial = friction_mtl_4;
        // }
        clone.transform.SetParent(parentObj);
        if (rot > 0f)
        {
            clone.transform.localScale = new Vector3(-1f * clone.transform.localScale.x, clone.transform.localScale.y, clone.transform.localScale.z);
        }
    }

}
