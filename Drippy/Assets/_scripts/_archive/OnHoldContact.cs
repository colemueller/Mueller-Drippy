using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnHoldContact : MonoBehaviour {

    public Text scoreText;
    private int score = 0;
    private AudioSource drop;
    private Rigidbody2D playerRigidbody;
    private GameObject currentHold;
    //public Camera cam;
    private bool doSway = false;
    public float swaySpeed = 5;
    public float swayDistance = 60;
    private float swing_mult;
    private float prev_angle;
    private float current_angle;
    bool canTap = false;

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        drop = GetComponent<AudioSource>();
    }

    private void Update()
    {
        scoreText.text = "Score: " + score.ToString();

        if (doSway)
        {
            // sway based on sin wave
            transform.localEulerAngles = new Vector3(0, 0, Mathf.Sin(Time.time *swaySpeed) * swayDistance);
            current_angle = transform.localEulerAngles.z;
            if (current_angle >= 180f) {
                //left side
                if (current_angle > prev_angle) {
                    swing_mult = -1;
                } else {
                    swing_mult = 1;
                }
            } else{
                //right side
                if (current_angle > prev_angle) {
                    swing_mult = 1;
                } else {
                    swing_mult = -1;
                }
            }
            prev_angle = current_angle;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("hold"))
        {
            currentHold = collision.gameObject;
            
            //cam.GetComponent<CameraFollow>().enabled = false;
            GetComponent<CircleCollider2D>().enabled = false;
            doSway = true;
            transform.position = new Vector3(currentHold.transform.position.x, currentHold.transform.position.y, transform.position.z);
            playerRigidbody.velocity = Vector2.zero;
            playerRigidbody.gravityScale = 0;
            score++;
            drop.Play();
            canTap = true;
        }
    }

    public void OnScreenTap()
    {
        if (canTap)
        {
            currentHold.tag = "Untagged";
            currentHold.GetComponent<CircleCollider2D>().enabled = true;

            //cam.GetComponent<CameraFollow>().enabled = true;
            GetComponent<CircleCollider2D>().enabled = true;
            doSway = false;
            playerRigidbody.gravityScale = 1;
            // 
            playerRigidbody.velocity = new Vector2(Mathf.Cos(Mathf.Deg2Rad*(transform.localEulerAngles.z-90f))*2f*swing_mult,
                                                   Mathf.Abs(Mathf.Cos(Mathf.Deg2Rad*(transform.localEulerAngles.z-90f)))*2f);
            canTap = false;
        }
    }
}
