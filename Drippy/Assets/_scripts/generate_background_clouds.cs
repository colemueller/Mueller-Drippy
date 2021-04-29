﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generate_background_clouds : MonoBehaviour
{
    public GameObject generator_object;
    public GameObject destroyer;
    public GameObject[] clouds;
    public int density;
    public float gen_x_min;
    public float gen_x_max;
    public float gen_y_separation_min;
    public float gen_y_separation_max;
    public float paralax_ratio;
    public float opacity_min;
    public float opacity_max;
    public float color_scale;
    public float cloud_scale;

    private Vector3 cam_pos;
    private Vector3 prev_cam_pos;
    private float last_cloud_y = 0f;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < density; i++) 
        { 
            GameObject random_cloud = pick_random_cloud();
            float x = Random.Range(gen_x_min, gen_x_max);
            float y = 0f;
            if (last_cloud_y != 0f)
            {
                y = last_cloud_y + Random.Range(gen_y_separation_min, gen_y_separation_max);
            }
            Vector3 spawn_pos = new Vector3(generator_object.transform.position.x + x, generator_object.transform.position.y + 5f - y, 5);
            GameObject last_cloud = Instantiate(random_cloud, spawn_pos, Quaternion.identity, this.transform);
            last_cloud.transform.localScale = new Vector3(cloud_scale, cloud_scale, cloud_scale);
            last_cloud.GetComponent<SpriteRenderer>().color = new Color(color_scale, color_scale, color_scale, Random.Range(opacity_min, opacity_max));
            last_cloud_y = last_cloud.transform.position.y;
        }
        prev_cam_pos = Camera.main.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        cam_pos = Camera.main.transform.position;
        while (this.transform.childCount < density)
        {
            GameObject random_cloud = pick_random_cloud();
            float x = Random.Range(gen_x_min, gen_x_max);
            float y = 0f;
            if (last_cloud_y != 0f)
            {
                y = last_cloud_y + Random.Range(gen_y_separation_min, gen_y_separation_max);
            }
            Debug.Log("Gen: " + generator_object.transform.position.y);
            Debug.Log(y);

            Vector3 spawn_pos = new Vector3(generator_object.transform.position.x + x, generator_object.transform.position.y, 5);
            GameObject last_cloud = Instantiate(random_cloud, spawn_pos, Quaternion.identity, this.transform);
            last_cloud.transform.localScale = new Vector3(cloud_scale, cloud_scale, cloud_scale);
            last_cloud.GetComponent<SpriteRenderer>().color = new Color(color_scale, color_scale, color_scale, Random.Range(opacity_min, opacity_max));
            last_cloud_y = last_cloud.transform.position.y;
        }

        Vector3 cam_pos_diff = cam_pos - prev_cam_pos;
        for (int i = 0; i < this.transform.childCount; i++)
        {
            GameObject cloud = this.transform.GetChild(i).gameObject;
            cloud.transform.position = new Vector3(cloud.transform.position.x, cloud.transform.position.y + cam_pos_diff.y * paralax_ratio, cloud.transform.position.z);
            if (cloud.transform.position.y > destroyer.transform.position.y + 50f)
            {
                Destroy(cloud);
            }
        }
        prev_cam_pos = cam_pos;
    }

    private GameObject pick_random_cloud()
    {
        int rand = Random.Range(0, clouds.Length);
        return clouds[rand];
    }
}