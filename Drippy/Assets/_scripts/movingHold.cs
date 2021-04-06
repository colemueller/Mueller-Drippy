using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingHold : MonoBehaviour
{
    public float move_distance;
    public float move_speed;
    private int move_direction = 1;
    private Vector3 orig_pos;


    // Start is called before the first frame update
    void Awake()
    {
        // random speed
        move_speed = Random.Range(move_speed, move_speed + .01f);

        // need to reset position so the movement doesn't take off screen
        float new_spawn = Random.Range(-1.25f, 1.25f);
        if (transform.position.x + move_distance > 1f)
        {
            transform.position = new Vector3(new_spawn, transform.position.y, transform.position.z);
        }
        if (transform.position.x - move_distance < -1f)
        {
            transform.position = new Vector3(new_spawn, transform.position.y, transform.position.z);
        }
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
