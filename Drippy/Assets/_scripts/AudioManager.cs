using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public static bool musicPlaying = false;
    public static AudioSource music;
    public static AudioSource rain;
    public static AudioSource drop;
    public static GameObject parGameObject;

    void Awake()
    {
        if(GameObject.FindGameObjectsWithTag("AudioManager").Length > 1)
        {
            GameObject.Destroy(this.gameObject);
        }
        else
        {
            music = this.transform.GetChild(0).GetComponent<AudioSource>();
            rain = this.transform.GetChild(1).GetComponent<AudioSource>();
            drop = this.transform.GetChild(2).GetComponent<AudioSource>();
            parGameObject = this.gameObject;
            GameObject.DontDestroyOnLoad(this.gameObject);
        }
    }

    public static void ToggleAltMusic(bool useAltMusic)
    {
        //print("hit");
        bool wasPlaying = false;
        if(musicPlaying)
        {
            music.Stop();
            musicPlaying = false;
            wasPlaying = true;
        }

        if(useAltMusic)
        {
            music = parGameObject.transform.GetChild(3).GetComponent<AudioSource>();
        }
        else
        {
            music = parGameObject.transform.GetChild(0).GetComponent<AudioSource>();
        }
        if(wasPlaying)
        {
            music.Play();
            musicPlaying = true;
        }
    }
}