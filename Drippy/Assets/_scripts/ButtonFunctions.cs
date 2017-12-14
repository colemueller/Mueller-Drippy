using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour {

    private AudioSource[] allAudioSources;

    public void OnStartClick()
    {
        SceneManager.LoadScene(1);
    }

    public void OnHomeClick()
    {
        StopAllAudio();
        SceneManager.LoadScene(0);
        
    }


    void StopAllAudio()
    {
        allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        foreach (AudioSource audioS in allAudioSources)
        {
            audioS.Stop();
        }
    }
}
