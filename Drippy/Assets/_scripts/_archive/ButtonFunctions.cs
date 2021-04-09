using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonFunctions : MonoBehaviour {

    private AudioSource[] allAudioSources;
    //public AudioSource rain, music, drop;
    public Sprite audio_playSprite;
    public Sprite audio_muteSprite;
    public Image muteBtnImage;

    public GameObject StartMenu, OptionsMenu, PauseMenu;
    public Slider musicVolSlider, sfxVolSlider, ambientVolSlider;
    public Toggle altMusicToggle;

    public Button optionsBackBtn;
    public GameObject pauseJunk;

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

        musicVolSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        sfxVolSlider.value = PlayerPrefs.GetFloat("SfxVolume");
        ambientVolSlider.value = PlayerPrefs.GetFloat("AmbientVolume");
        if(PlayerPrefs.GetInt("UseAltMusic") == 1 && StartGame.isStart)
        {
            //Mute(altMusicToggle.onValueChanged);
            altMusicToggle.isOn = true;
            //Unmute(altMusicToggle.onValueChanged);
        }

    }

    public void Restart(bool isStart)
    {
        Score._score = 0;
        OnDeath.isDead = false;
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

    public void ShowOptions(bool show)
    {
        //print(show);
        StartMenu.SetActive(!show);
        OptionsMenu.SetActive(show);
    }

    public void UpdateMusicVol(float val)
    {
        PlayerPrefs.SetFloat("MusicVolume",val);

        AudioManager.music.volume = val;
        if(AudioManager.music.isPlaying == false && StartGame.isStart == false)
        {
            AudioManager.music.Play();
            AudioManager.musicPlaying = true;
        }
    }
    public void UpdateSfxVol(float val)
    {
        PlayerPrefs.SetFloat("SfxVolume",val);
        AudioManager.drop.volume = val;
    }
    public void UpdateAmbientVol(float val)
    {
        PlayerPrefs.SetFloat("AmbientVolume",val);
        AudioManager.rain.volume = val;
        if(AudioManager.rain.isPlaying == false)
        {
            AudioManager.rain.Play();
        }
    }

    public void doPause(bool show)
    {
        optionsBackBtn.onClick.RemoveAllListeners();
        if(show)
        {
            Time.timeScale = 0;
            optionsBackBtn.onClick.AddListener(delegate{pauseShowOptions(false);});
        }
        else
        {
            optionsBackBtn.onClick.AddListener(delegate{ShowOptions(false);});
            Time.timeScale = 1;
        }
        
        PauseMenu.SetActive(show);
    }

    public void quitGame()
    {
        doPause(false);
        Restart(true);
    }

    public void pauseShowOptions(bool show)
    {
        pauseJunk.SetActive(!show);
        OptionsMenu.SetActive(show);
    }

    void OnApplicationFocus(bool focus)
    {
        //Debug.Log("FOCUS: "+ focus + StartGame.isStart);
        if(focus == false && StartGame.isStart == false && !OnDeath.isDead)
        {
            doPause(true);
        }

    }

    public void ToggleAltMusic()
    {
        AudioManager.ToggleAltMusic(altMusicToggle.isOn);
        if(altMusicToggle.isOn)
        {
            PlayerPrefs.SetInt("UseAltMusic",1);
        }
        else
        {
            PlayerPrefs.SetInt("UseAltMusic",0);
        }
    }

     public void Mute( UnityEngine.Events.UnityEventBase ev )
    {
        int count = ev.GetPersistentEventCount();
        for ( int i = 0 ; i < count ; i++ )
        {
            ev.SetPersistentListenerState( i, UnityEngine.Events.UnityEventCallState.Off );
        }
    }
    
    public void Unmute( UnityEngine.Events.UnityEventBase ev )
    {
        int count = ev.GetPersistentEventCount();
        for ( int i = 0 ; i < count ; i++ )
        {
            ev.SetPersistentListenerState( i, UnityEngine.Events.UnityEventCallState.RuntimeOnly );
        }
    }
    
}
