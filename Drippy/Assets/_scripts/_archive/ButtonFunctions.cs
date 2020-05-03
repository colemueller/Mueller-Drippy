using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonFunctions : MonoBehaviour {

    private AudioSource[] allAudioSources;
    public Sprite audio_playSprite;
    public Sprite audio_muteSprite;
    public Image muteBtnImage;

    public void Start()
    {
        if(!PlayerPrefs.HasKey("IsMuted"))
        {
            PlayerPrefs.SetInt("IsMuted", 0);
        }

        if(PlayerPrefs.GetInt("IsMuted") == 1)
        {
            allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
            foreach (AudioSource audioS in allAudioSources)
            {
                //audioS.Stop();
                audioS.mute = true;
            }
            muteBtnImage.sprite = audio_playSprite;
        }
    }

    public void Restart(bool isStart)
    {
        Score._score = 0;
        if(isStart)
        {
            StartGame.isStart = true;
        }
        else
        {
            StartGame.isStart = false;
        }
        SceneManager.LoadScene(0);
    }

    public void MuteToggle()
    {
        allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        int isMuted = PlayerPrefs.GetInt("IsMuted");
        bool tmp;
        if(isMuted == 1)
        {
            tmp = false;
            muteBtnImage.sprite = audio_muteSprite;
            PlayerPrefs.SetInt("IsMuted",0);
        }
        else
        {
            tmp = true;
            muteBtnImage.sprite = audio_playSprite;
            PlayerPrefs.SetInt("IsMuted",1);
        }

        foreach (AudioSource audioS in allAudioSources)
        {
            //audioS.Stop();
            audioS.mute = tmp;
        }
    }
}
