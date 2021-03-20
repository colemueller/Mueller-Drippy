using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    public static bool isStart = true;
    private GameObject startMenu;
    public Rigidbody2D playerRB;
    private RectTransform playerTrans;
    public AudioSource music;
    public AudioSource rain;
    public AudioSource drop;
    public GameObject InGameUI;
    private bool moveDrippy = true;
    public float rotSpeed;
    public float moveSpeed;
    public float moveDist;
    private bool moveBack = false;
    private float startTime;
    public Transform startHold;
    private int randStart;


    void Awake()
    {
        if(!PlayerPrefs.HasKey("MusicVolume"))
        {
            PlayerPrefs.SetFloat("MusicVolume",0.8f);
            PlayerPrefs.SetFloat("SfxVolume",0.8f);
            PlayerPrefs.SetFloat("AmbientVolume",0.8f);
        }

        rain.volume = PlayerPrefs.GetFloat("AmbientVolume");
        rain.Play();
        drop.volume = PlayerPrefs.GetFloat("SfxVolume");

        startMenu = this.gameObject;
        playerTrans = playerRB.gameObject.GetComponent<RectTransform>();
        if(isStart == false)
        {
            moveDrippy = false;
            OnStartPress();
        }
        else
        {
            randStart = Random.Range(0,2)*2-1;
            //print(randStart);
            moveDrippy = true;
            InGameUI.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(moveDrippy)
        {
            /* if(moveBack)
            {
                float t = (Time.time - startTime) / 1f;
                playerTrans.position = new Vector3(Mathf.SmoothStep(playerTrans.position.x, 0, t),playerTrans.position.y,1);
                
                if(playerTrans.position.x == 0)
                {
                    moveBack = false;
                    moveDrippy = false;
                    OnStartPress();
                }
            }
            else
            { */
                playerTrans.localEulerAngles = new Vector3(0, 0, Mathf.Sin(Time.time * rotSpeed) * 40);
                //playerTrans.localEulerAngles = new Vector3(0,0,playerTrans.localEulerAngles.z + rotSpeed);
                playerTrans.position = new Vector3((Mathf.Sin(Time.time * moveSpeed) * moveDist) * randStart,playerTrans.position.y,1);
            //}
            
        }
    }

    /* public void MoveBack()
    {
        moveBack = true;
        startTime = Time.time;
    } */

    public void OnStartPress()
    {
        isStart = false;
        moveDrippy = false;
        startHold.position = new Vector3(playerTrans.position.x,startHold.position.y,startHold.position.z);
        startMenu.SetActive(false);
        playerRB.gravityScale = 1;
        if(PlayerPrefs.GetFloat("MusicVolume") != 0.0f)
        {
            music.volume = PlayerPrefs.GetFloat("MusicVolume");
            //print(music.volume);
            music.Play();
        }
        drop.volume = PlayerPrefs.GetFloat("SfxVolume");
        //rain.volume = 0.5f;
        InGameUI.SetActive(true);
    }
}
