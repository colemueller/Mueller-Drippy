using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generate : MonoBehaviour {

    public GameObject holdPrefab;
    public GameObject platformPrefab;
    public Transform parentObj;
    public PhysicsMaterial2D friction_mtl_0;
    public PhysicsMaterial2D friction_mtl_2;
    public PhysicsMaterial2D friction_mtl_4;

    private int spawnNum = 5;
    private int dot_streak = 0;
    private int platform_streak = 0;
    private Color current_color;
    private float color_change_r;
    private float color_change_g;
    private float color_change_b;

    public void Start()
    {
        spawnNum = 5;

        current_color = holdPrefab.GetComponent<SpriteRenderer>().color;
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
            //random color changing
            color_change_r = Random.Range(-.12f, .12f);
            color_change_g = Random.Range(-.1f, .1f);
            color_change_b = Random.Range(-.15f, .15f);

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
                GenerateHold(randx);
            }
            spawnNum = spawnNum + 4;
        }
    }

    public void GenerateHold(float rand)
    {
        GameObject clone = Instantiate(holdPrefab, new Vector3(rand, transform.position.y, 2), Quaternion.identity) as GameObject;
        clone.transform.SetParent(parentObj);

        current_color = new Color(current_color[0] + color_change_r, current_color[1] + color_change_g, current_color[2] + color_change_b, 1f);
        clone.GetComponent<SpriteRenderer>().color = current_color;
    }
    

    public void GeneratePlatform(float rand, float angle_min, float angle_max)
    {
        float rot = Random.Range(angle_min, angle_max);
        GameObject clone = Instantiate(platformPrefab, new Vector3(rand, transform.position.y, 2), Quaternion.Euler(0f, 0f, rot)) as GameObject;
        if (Mathf.Abs(rot) <= 15) 
        {
            clone.GetComponent<BoxCollider2D>().sharedMaterial = friction_mtl_0;
            clone.GetComponent<Rigidbody2D>().sharedMaterial = friction_mtl_0;
        }
        else if (Mathf.Abs(rot) > 15 && Mathf.Abs(rot) <= 25)
        {
            clone.GetComponent<BoxCollider2D>().sharedMaterial = friction_mtl_2;
            clone.GetComponent<Rigidbody2D>().sharedMaterial = friction_mtl_2;
        }
        else
        {
            clone.GetComponent<BoxCollider2D>().sharedMaterial = friction_mtl_4;
            clone.GetComponent<Rigidbody2D>().sharedMaterial = friction_mtl_4;
        }
        clone.transform.SetParent(parentObj);

        current_color = new Color(current_color[0] + color_change_r, current_color[1] + color_change_g, current_color[2] + color_change_b, 1f);
        clone.GetComponent<SpriteRenderer>().color = current_color;
    }

}
