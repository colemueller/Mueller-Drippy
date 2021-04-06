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
    private bool doSway = false;
    private float swing_mult;
    private float prev_angle;
    private float current_angle;
    private RectTransform myTransform;
    private float velocity_zero_timer = 0f;
    private Vector3 startScale;
    private ParticleSystem[] splash_particles;
    private ParticleSystem cloud_puff_particles;
    private bool on_platform = false;
    private Animator player_animator;
    public bool canTap = false;
    public float swaySpeed = 5;
    public float swayDistance = 60;
    public float maxFallSpeed = 10f;
    public float maxSideSpeed = 8f;
    public float x_force_mult = 3.5f;
    public float y_force_mult = .5f;
    private Vector3 cam_pos;
    public float max_shake = 1f;
    private float min_shake;
    private bool shake_left;
    private bool shake_up;
    public SpriteRenderer eyes;

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        drop = GetComponent<AudioSource>();
        myTransform = this.GetComponent<RectTransform>();
        startScale = transform.localScale;
        //player_animator = gameObject.GetComponentInChildren<Animator>();
        player_animator = transform.GetChild(0).GetComponent<Animator>();
        cam_pos = new Vector3(0, (myTransform.position.y - 3.5f), 0);
        min_shake = -max_shake;
        shake_left = true;
        shake_up = true;
    }

    private void Update()
    {
        // factor cam shake from high speed into cam pos
        max_shake = playerRigidbody.velocity.y.Remap(-10f, -20f, .005f, max_shake);
        if (playerRigidbody.velocity.y / -maxFallSpeed >= .5f)
        {
            Vector3 new_pos = cam_pos;
            float inc = max_shake / 2f;
            if (shake_left)
            {
                if (new_pos.x <= min_shake)
                {
                    shake_left = false;
                }
                else
                {
                    inc *= -1f;
                }
            }
            else
            {
                if (new_pos.x >= max_shake)
                {
                    shake_left = true;
                }
                else
                {
                    inc *= 1f;
                }
            }
            new_pos = new Vector3(cam_pos.x + inc, cam_pos.y, cam_pos.z);

            inc = max_shake / 5f;
            if (shake_up)
            {
                if (cam_pos.y >= new_pos.y + max_shake)
                {
                    shake_up = false;
                }
                else
                {
                    inc *= -1f;
                }
            }
            else
            {
                if (cam_pos.y <= new_pos.y - max_shake)
                {
                    shake_up = true;
                }
                else
                {
                inc *= 1f;
                }
            }
            new_pos = new Vector3(new_pos.x, (myTransform.position.y - 3.5f) + inc, cam_pos.z);
            Camera.main.transform.position = new Vector3(new_pos.x, new_pos.y, new_pos.z);
        }
        else
        {
            Camera.main.transform.position = new Vector3(0, (myTransform.position.y - 3.5f), 0);
        }
        // end cam shake //

        // set cam pos
        cam_pos = Camera.main.transform.position;

        score = myTransform.position.y - 3;
        score = Mathf.RoundToInt(score);
        score = Mathf.Abs(score);
        Score._score = (int)score;

        if (doSway)
        {
            transform.position = new Vector3(currentHold.transform.position.x, currentHold.transform.position.y - 0.2f, transform.position.z);
            playerRigidbody.angularVelocity = 0f;
            // sway based on sin wave
            float newAngle = Mathf.Sin(Time.time * swaySpeed) * swayDistance;
            transform.localEulerAngles = new Vector3(0, 0, newAngle);
            // determines if the player is on an up or down swing for use in the OnScreenTap y-velocity
            current_angle = transform.localEulerAngles.z;
            eyes.enabled = false;

            if(current_angle != prev_angle)
            {
                float newNum = Mathf.Round((current_angle-prev_angle) * 10.0f) * 0.1f;
                if(current_angle > 300 && prev_angle < 10) //just crossed mid from right to left
                {
                    player_animator.SetBool("goRight",false);
                }
                else if(current_angle < 10 && prev_angle > 300) //just crossed mid from left to right
                {
                    player_animator.SetBool("goRight",true);
                }
                else
                {
                    if(newNum > 0) //start going right
                    {
                        player_animator.SetBool("goRight",true);
                    }
                    if(newNum < 0) //start going left
                    {
                        player_animator.SetBool("goRight",false);
                    }
                } 
            }
            
            
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
            if (Mathf.Abs(playerRigidbody.velocity.y) >= maxFallSpeed)
            {
                playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x,
                                                                              -maxFallSpeed);
            }
            if (Mathf.Abs(playerRigidbody.velocity.x) >= maxSideSpeed)
            {
                if (playerRigidbody.velocity.x > 0)
                {
                    playerRigidbody.velocity = new Vector2(maxSideSpeed, playerRigidbody.velocity.y);
                }
                else {
                    playerRigidbody.velocity = new Vector2(-maxSideSpeed, playerRigidbody.velocity.y);
                }
            }
            if (on_platform)
            {
                if (currentPlatform != null)
                {
                    transform.rotation = new Quaternion(0f, 0f, currentPlatform.transform.rotation.z, Quaternion.identity[3]);
                }
                
                if (playerRigidbody.velocity.x < .0f)
                {
                    // animate to left slide
                    player_animator.ResetTrigger("left_to_fall_trigger");
                    player_animator.SetTrigger("left_drip_trigger");
                }
                else if (playerRigidbody.velocity.x > 0f)
                {
                    // animate to right slide
                    player_animator.ResetTrigger("right_to_fall_trigger");
                    player_animator.SetTrigger("right_drip_trigger");
                }
                player_animator.SetFloat("velocity_flip", playerRigidbody.velocity.x);
            }
            else
            {
                // animate to fall
                if (player_animator.GetCurrentAnimatorStateInfo(0).IsName("left_slide"))
                {
                    player_animator.ResetTrigger("left_drip_trigger");
                    player_animator.ResetTrigger("right_drip_trigger");

                    player_animator.SetTrigger("left_to_fall_trigger");
                }
                else if (player_animator.GetCurrentAnimatorStateInfo(0).IsName("right_slide"))
                {
                    player_animator.ResetTrigger("left_drip_trigger");
                    player_animator.ResetTrigger("right_drip_trigger");

                    player_animator.SetTrigger("right_to_fall_trigger");
                }
                if (this.GetComponent<RectTransform>().eulerAngles.z != 0f)
                {
                    if (180f > this.GetComponent<RectTransform>().eulerAngles.z && this.GetComponent<RectTransform>().eulerAngles.z > 0f)
                    {
                        this.GetComponent<RectTransform>().eulerAngles = new Vector3(this.GetComponent<RectTransform>().eulerAngles.x,
                                                                                    this.GetComponent<RectTransform>().eulerAngles.y,
                                                                                    this.GetComponent<RectTransform>().eulerAngles.z - .25f);
                    }
                    if (this.GetComponent<RectTransform>().eulerAngles.z < 360f && this.GetComponent<RectTransform>().eulerAngles.z > 180f)
                    {
                        this.GetComponent<RectTransform>().eulerAngles = new Vector3(this.GetComponent<RectTransform>().eulerAngles.x,
                                                                                    this.GetComponent<RectTransform>().eulerAngles.y,
                                                                                    this.GetComponent<RectTransform>().eulerAngles.z + .25f);
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("hold"))
        {
            if (on_platform)
            {
                on_platform = false;
            }
            currentHold = collision.gameObject;
            
            // puffy puff when hit hold
            cloud_puff_particles = currentHold.GetComponentInChildren<ParticleSystem>();
            cloud_puff_particles.Play();

            

            GetComponent<CircleCollider2D>().enabled = false;
            doSway = true;
            player_animator.SetBool("doSway",true);
            transform.position = new Vector3(currentHold.transform.position.x, currentHold.transform.position.y - 0.2f, transform.position.z);
            playerRigidbody.velocity = Vector2.zero;
            playerRigidbody.gravityScale = 0;
            
            currentHold.GetComponentInChildren<Animator>().SetTrigger("doLand");

            drop.Play();
            canTap = true;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "platform")
        {
            on_platform = true;
            currentPlatform = collision.gameObject;
            playerRigidbody.angularVelocity = 0f;
            // transform.rotation = new Quaternion(0f, 0f, collision.gameObject.transform.rotation.z, Quaternion.identity[3]);
            playerRigidbody.freezeRotation = true;

            // Determine how much to move drippy up so he doesn't rotate through the platform
            Vector3 contact_point = collision.GetContact(0).point;
            float yOffset = transform.position.y - contact_point.y;
            if(yOffset > .5f) yOffset = .5f;
            float offset = .5f - yOffset;
            transform.position = new Vector3(transform.position.x, transform.position.y + offset, transform.position.z);

            // Splashy splash when hit platform
            splash_particles = currentPlatform.GetComponentsInChildren<ParticleSystem>();
            foreach(ParticleSystem p in splash_particles)
            {
                p.transform.position = contact_point;
                p.Play();
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("platform_edge"))
        {
            on_platform = false;
            playerRigidbody.freezeRotation = false;
            playerRigidbody.angularVelocity = 90f;
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
            eyes.enabled = true;
            player_animator.SetBool("goRight",false);
            player_animator.SetBool("doSway",false);
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


public static class ExtensionMethods {
 
    public static float Remap (this float value, float from1, float to1, float from2, float to2) {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
   
}