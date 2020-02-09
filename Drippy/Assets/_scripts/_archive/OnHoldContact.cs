using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnHoldContact : MonoBehaviour {

    public Text scoreText;
    private float score = 0;
    private AudioSource drop;
    private Rigidbody2D playerRigidbody;
    private GameObject currentHold;
    private GameObject currentPlatform;
    //public Camera cam;
    private bool doSway = false;
    public float swaySpeed = 5;
    public float swayDistance = 60;
    public float x_force_mult = 3.5f;
    public float y_force_mult = .5f;
    private float swing_mult;
    private float prev_angle;
    private float current_angle;
    bool canTap = false;
    private RectTransform myTransform;

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        drop = GetComponent<AudioSource>();
        myTransform = this.GetComponent<RectTransform>();
    }

    private void Update()
    {
        score = myTransform.position.y;
        score = Mathf.RoundToInt(score);
        score = Mathf.Abs(score);
        scoreText.text = "Score: " + score.ToString() + "m";

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
            //score++;
            drop.Play();
            canTap = true;
        }
        else if (collision.CompareTag("platform_edge"))
        {
            currentPlatform.GetComponent<BoxCollider2D>().enabled = false;
            // put the transition back to falling sprite here
            transform.localScale = new Vector3(100, 100, 100);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "platform")
        {
            currentPlatform = collision.gameObject;
            BoxCollider2D[] side_cols = currentPlatform.GetComponentsInChildren<BoxCollider2D>();
            foreach (BoxCollider2D bc in side_cols) {
                bc.enabled = true;
            }
            // put the transition to slide sprite here
            transform.localScale = new Vector3(100, 50, 100);

            Debug.Log("platform");
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
            playerRigidbody.velocity = new Vector2(Mathf.Cos(Mathf.Deg2Rad*(transform.localEulerAngles.z-90f))*x_force_mult*swing_mult,
                                                   (Mathf.Abs(Mathf.Cos(Mathf.Deg2Rad*(transform.localEulerAngles.z-90f)))+y_force_mult)*2f*swing_mult);
            canTap = false;
        }
    }
}
