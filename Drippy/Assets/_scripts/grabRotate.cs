using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grabRotate : MonoBehaviour
{

    private Collider2D platform_col;
    private Transform platform_transform;
    private Vector3 drag_origin;
    private float drag_dist;
    private bool new_down;
    // Start is called before the first frame update
    void Start()
    {
        platform_col = this.GetComponentInParent<Collider2D>();
        platform_transform = this.GetComponentsInParent<Transform>()[1];
        new_down = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (new_down)
            {
                drag_origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                drag_origin = new Vector3(drag_origin.x, drag_origin.y, 2f);
                new_down = false;
            }

            if (platform_col.bounds.Contains(drag_origin))
            {
                drag_dist = Vector2.Angle(drag_origin, Camera.main.ScreenToWorldPoint(Input.mousePosition));
                Debug.Log(this.GetComponentsInParent<Transform>()[1].gameObject.ToString());
                platform_transform.Rotate(0f, 0f, drag_dist);
            }
        }
        else
        {
            new_down = true;
        }
    }
}

// {
//     public float dragSpeed = 2;
//     private Vector3 dragOrigin;
 
 
//     void Update()
//     {
//         if (Input.GetMouseButtonDown(0))
//         {
//             dragOrigin = Input.mousePosition;
//             return;
//         }
 
//         if (!Input.GetMouseButton(0)) return;
 
//         Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
//         Vector3 move = new Vector3(pos.x * dragSpeed, 0, pos.y * dragSpeed);
 
//         transform.Translate(move, Space.World);  
//     }
 
 
// }