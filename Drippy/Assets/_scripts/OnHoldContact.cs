using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnHoldContact : MonoBehaviour {

    public Generate genScript;
    public Text scoreText;
    private int score = 0;
    private AudioSource drop;
    private Rigidbody2D playerRigidbody;
    private GameObject currentHold;
    //public Camera cam;
    public Transform forceTransform;
    private bool doSway = false;
    public float swaySpeed = 5;
    public float swayDistance = 60;
    public Vector2 forceVars;
    private Vector2 forceAmount;
    private Vector2 forceVector;
    public bool doBoost = true;
    public float boostAmount;

    bool canTap = false;

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        drop = GetComponent<AudioSource>();
    }

    private void Update()
    {
        scoreText.text = "Score: " + score.ToString();

        //Increases force amount as the swing approaches the edges
        if (doBoost)
        {
            forceVars = new Vector2(forceVars.x + (Mathf.Sin(Time.time * swaySpeed) * boostAmount), forceVars.y);
        }

        forceVector = new Vector2(forceTransform.position.x, forceTransform.position.y);
        forceAmount = new Vector2(Mathf.Sin(Time.time * swaySpeed) * forceVars.x, forceVars.y);

        if (doSway)
        {
            transform.localEulerAngles = new Vector3(0, 0, Mathf.Sin(Time.time *swaySpeed) * swayDistance);
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
            forceTransform.gameObject.GetComponent<MoveThis>().enabled = true;
            transform.position = new Vector3(currentHold.transform.position.x, currentHold.transform.position.y, transform.position.z);
            playerRigidbody.velocity = Vector2.zero;
            playerRigidbody.gravityScale = 0;
            score++;
            drop.Play();
            //genScript.GenerateHold();
            canTap = true;
            Debug.Log("HIT");
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

            playerRigidbody.AddForceAtPosition(forceAmount, forceVector, ForceMode2D.Impulse);
            forceTransform.gameObject.GetComponent<MoveThis>().enabled = false;
            canTap = false;
            Debug.Log("FALL");
        }
    }
}
