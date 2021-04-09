using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grabRotate : MonoBehaviour
{
    public bool moving_platform;
    public float max_rotate = 60f;
    private Collider2D platform_col;
    private Transform platform_transform;
    private Vector3 drag_origin;
    private float drag_dist;
    private bool new_down;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        platform_col = this.GetComponentInParent<Collider2D>();
        platform_transform = this.GetComponentsInParent<Transform>()[1];
        new_down = true;
        moving_platform = false;
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !moving_platform)
        {
            drag_origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (platform_col.bounds.Contains(new Vector3(drag_origin.x, drag_origin.y, 2f)))
            {
                moving_platform = true;
                player.GetComponent<OnHoldContact>().canTap = false;
            }
        }
        if (Input.GetMouseButton(0) && moving_platform)
        {
            Vector3 mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector3 aimDirection = (mouse_pos - platform_transform.position).normalized;
            float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

            if (platform_transform.localScale.x < 0f)
            {
                //Debug.Log(angle);
                if (angle <= 180f - max_rotate && angle > 0f)
                {
                    angle = -max_rotate;
                }
                else if (angle >= -180f + max_rotate && angle < 0f)
                {
                    angle = max_rotate;
                }
                else
                {
                    angle -= 180f;
                }
            }
            else
            {
                if (angle >= max_rotate)
                {
                    angle = max_rotate;
                }
                else if (angle <= -max_rotate)
                {
                    angle = -max_rotate;
                }
            }
            platform_transform.eulerAngles = new Vector3(0f, 0f, angle);
        }
        if (Input.GetMouseButtonUp(0))
        {
            player.GetComponent<OnHoldContact>().canTap = true;
            moving_platform = false;
        }
    }
}