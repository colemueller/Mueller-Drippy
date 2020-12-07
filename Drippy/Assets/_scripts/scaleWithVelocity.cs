using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scaleWithVelocity : MonoBehaviour
{
    private RectTransform player_transform;
    private Rigidbody2D player_rb;
    private float max_vel = -20f;
    private float min_scale = .5f;
    public float shake_amt = .01f;
    float shake = 0;
    bool shake_left = true;

    // Start is called before the first frame update
    void Start()
    {
        player_transform = this.GetComponent<RectTransform>();
        player_rb = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float norm_vel = player_rb.velocity.y / max_vel;
        float x_scale = .6f - (norm_vel * .6f);
        if (x_scale < min_scale)
        {
            x_scale = min_scale;
        }
        player_transform.localScale = new Vector3(x_scale, .6f, 1);
       
        // l/r sprite shake
        if (Mathf.Abs(player_rb.velocity.y) >= Mathf.Abs(max_vel * .3f))
        {
            if (shake_left)
            {
                if (shake >= shake_amt)
                {
                    shake_left = false;
                }
                else
                {
                    shake += shake_amt * .5f;
                }
            }
            else
            {
                if (shake <= -shake_amt)
                {
                    shake_left = true;
                }
                else{
                    shake -= shake_amt * .5f;
                }
            }
            this.GetComponentInChildren<Transform>().position = new Vector3(shake, this.GetComponentInChildren<Transform>().position.y, this.GetComponentInChildren<Transform>().position.z);
        }
        else
        {
            this.GetComponentInChildren<Transform>().position = new Vector3(0f, this.GetComponentInChildren<Transform>().position.y, this.GetComponentInChildren<Transform>().position.z);
        }
    }
}
