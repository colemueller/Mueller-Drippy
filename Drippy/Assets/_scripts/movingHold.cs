using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingHold : MonoBehaviour
{
    public float move_distance = 5f;
    public float move_speed = .5f;
    private int move_direction = 1;
    private Vector3 orig_pos;


    // Start is called before the first frame update
    void Awake()
    {
        orig_pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        if (pos.x >= orig_pos.x + move_distance || pos.x <= orig_pos.x - move_distance)
        {
            move_direction *= -1;
        }

        transform.position = new Vector3(pos.x + move_speed * move_direction, pos.y, pos.z);
    }
}
