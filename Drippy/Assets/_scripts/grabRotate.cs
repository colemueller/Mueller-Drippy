using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grabRotate : MonoBehaviour
{

    private Transform platform;
    private Touch the_touch;
    // Start is called before the first frame update
    void Start()
    {
        platform = this.GetComponentInParent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Debug.Log(Input.touchCount);
            the_touch = Input.GetTouch(0);

            if (the_touch.phase == TouchPhase.Began)
            {
                if (the_touch.position.x <= this.GetComponent<BoxCollider2D>().bounds.max.x && the_touch.position.y <= this.GetComponent<BoxCollider2D>().bounds.max.y &&
                    the_touch.position.x >= this.GetComponent<BoxCollider2D>().bounds.min.x && the_touch.position.y >= this.GetComponent<BoxCollider2D>().bounds.min.y)
                {
                    Debug.Log("Touch");
                }
            }
        }
    }
}
