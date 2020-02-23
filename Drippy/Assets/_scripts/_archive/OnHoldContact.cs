using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnHoldContact : MonoBehaviour {

    //public Score scoreText;
    private float score = 0;
    private AudioSource drop;
    private Rigidbody2D playerRigidbody;
    private GameObject currentHold;
    private GameObject currentPlatform;
    //public Camera cam;
    private bool doSway = false;
    private float swing_mult;
    private float prev_angle;
    private float current_angle;
    bool canTap = false;
    private RectTransform myTransform;
    private Vector3 startScale;
    public float swaySpeed = 5;
    public float swayDistance = 60;
    public float maxFallSpeed = 10f;
    public float maxSideSpeed = 8f;
    public float x_force_mult = 3.5f;
    public float y_force_mult = .5f;

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        drop = GetComponent<AudioSource>();
        myTransform = this.GetComponent<RectTransform>();
        startScale = transform.localScale;
    }

    private void Update()
    {
        Camera.main.transform.position = new Vector3(0, (myTransform.position.y - 3.5f), 0);
        score = myTransform.position.y - 3;
        score = Mathf.RoundToInt(score);
        score = Mathf.Abs(score);
        Score._score = (int)score;
        //scoreText.text = "Score: " + score.ToString() + "m";

        if (doSway)
        {
            // sway based on sin wave
            transform.localEulerAngles = new Vector3(0, 0, Mathf.Sin(Time.time * swaySpeed) * swayDistance);
            // determines if the player is on an up or down swing for use in the OnScreenTap y-velocity
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
        else 
        {
            if (Mathf.Abs(gameObject.GetComponent<Rigidbody2D>().velocity.y) >= maxFallSpeed) {
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(gameObject.GetComponent<Rigidbody2D>().velocity.x,
                                                                              -maxFallSpeed);
            }
            else if (Mathf.Abs(gameObject.GetComponent<Rigidbody2D>().velocity.x) >= maxSideSpeed) {
                if (gameObject.GetComponent<Rigidbody2D>().velocity.x > 0) {
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(maxSideSpeed, gameObject.GetComponent<Rigidbody2D>().velocity.y);
                }
                else {
                    gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(-maxSideSpeed, gameObject.GetComponent<Rigidbody2D>().velocity.y);
                }
            }
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
            transform.localScale = startScale;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "platform")
        {
            collision.gameObject.tag = "collidedPlatform";
            currentPlatform = collision.gameObject;
            BoxCollider2D[] side_cols = currentPlatform.GetComponentsInChildren<BoxCollider2D>();
            foreach (BoxCollider2D bc in side_cols) {
                bc.enabled = true;
            }
            // put the transition to slide sprite here
            transform.localScale = new Vector3(startScale.x, startScale.y*.1f, startScale.z);

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
            // x velocity is determined by the cosine value of the player's angle at time of tap.
            //      ___
            //    /     \   // -90 degrees makes the 0 degrees at the bottom of the circle
            //   |   |---|  // Cosine of the angle in radians gives a value of -1 to 1 determining which side the player is on
            //    \__|__/   // Multiply by the x_force_mult for tweaking
            //     
            // y velocity is determined by the same principle, but we take the abs value to determine if the player should have a y-velocity
            // We also add a y_force_mult for tweaking
            // Finally multiplying by a swing_mult to determine if the player was on an up swing or down swing (making the final y-vel + or - accordingly)
            playerRigidbody.velocity = new Vector2(Mathf.Cos(Mathf.Deg2Rad*(transform.localEulerAngles.z-90f))*x_force_mult*swing_mult,
                                                   (Mathf.Abs(Mathf.Cos(Mathf.Deg2Rad*(transform.localEulerAngles.z-90f)))+y_force_mult)*2f*swing_mult);
            // decides how much the player rotates when letting go of a hold (purely visual, doesn't do anything physically)
            playerRigidbody.angularVelocity = playerRigidbody.velocity.x * 150f;
            canTap = false;
        }
    }
}
