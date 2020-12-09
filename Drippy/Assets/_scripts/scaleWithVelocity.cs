using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scaleWithVelocity : MonoBehaviour
{
    public Transform main_camera;
    public float max_shake = 1f;
    private float min_shake;
    private RectTransform player_transform;
    private Rigidbody2D player_rb;
    private float max_vel = -20f;
    private float min_scale = .4f;

    // Start is called before the first frame update
    void Start()
    {
        player_transform = this.GetComponent<RectTransform>();
        player_rb = this.GetComponent<Rigidbody2D>();
        min_shake = -max_shake;
    }

    // Update is called once per frame
    void Update()
    {
        float norm_vel = player_rb.velocity.y / max_vel;
        float x_scale = .6f - (norm_vel * (.6f - min_scale));
        float y_scale = .6f + (norm_vel * (.6f - min_scale));
        if (x_scale < min_scale)
        {
            x_scale = min_scale;
            y_scale = .6f + min_scale/2f;
        }
        player_transform.localScale = new Vector3(x_scale, y_scale, 1);

        if (player_rb.velocity.y / max_vel >= .8f)
        {
            Vector3 cam_pos = main_camera.position;
            Vector3 new_pos = cam_pos;
            float inc = max_shake / 10f;
            if (cam_pos.x >= max_shake)
            {
                inc *= -1f;
            }
            else if (cam_pos.x <= min_shake)
            {
                inc *= -1f;
            }
            new_pos = new Vector3(cam_pos.x + inc, cam_pos.y, cam_pos.z);
            if (cam_pos.z >= max_shake)
            {
                inc *= -1f;
            }
            else if (cam_pos.z <= min_shake)
            {
                inc *= -1f;
            }
            new_pos = new Vector3(new_pos.x, cam_pos.y, cam_pos.z + inc);
            main_camera.position = new_pos;
            Debug.Log("Cam:" + cam_pos);
            Debug.Log("New:" + new_pos);
        }
        else
        {
            if (main_camera.position.x != 0f || main_camera.position.z != 0f)
            {
                main_camera.position = new Vector3(0f, main_camera.position.y, 0f);
            }
        }
    }
}
