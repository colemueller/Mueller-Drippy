using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnDeath : MonoBehaviour
{
    // Start is called before the first frame update
    public Text[] scores = new Text[5];
    public GameObject ingameUI;
    private int currScore;

    private bool setNewHighScore = false;
    private int newHighscoreNum;
    private bool doWiggle = false;
    private RectTransform wiggleMe = null;
    private Text MainText;
    public static bool isDead = false;

    public void SetUp()
    {
        EventManager.StartListening("death", InvokeDeath);
        MainText = this.transform.Find("MainText").GetComponent<Text>();
        this.gameObject.SetActive(true);
        isDead = false;
    }

    void Update()
    {
        //print(doWiggle);
        if(doWiggle == true)
        {
            //print("Wiggling!");
            //print(Mathf.Lerp(-3, 3, Mathf.PingPong(Time.time , 1)));
            //3 deg max
            wiggleMe.localEulerAngles = new Vector3(0,0, Mathf.Lerp(-5, 5, Mathf.PingPong(Time.time*2 , 1f)));
            //scale between 1.25 and 1
            wiggleMe.localScale = new Vector3(Mathf.Lerp(1, 1.25f, Mathf.PingPong(Time.time , 1f)),Mathf.Lerp(1, 1.25f, Mathf.PingPong(Time.time , 1f)),1);
        }
    }

    public void InvokeDeath()
    {  
        isDead = true;
        ingameUI.SetActive(false);

        //evaluate score and save high scores
        currScore = Score._score;
        SetHighScores(1);

        //print high scores
        for(int i = 1; i <=5; i++)
        {
            scores[i-1].text = i.ToString() + ". " + PlayerPrefs.GetInt("HighScore_"+i.ToString()) + "m";
        }
        MainText.text = currScore.ToString() + " m";
        this.gameObject.SetActive(true);
        if(setNewHighScore)
        {
            doWiggle = true;
            setNewHighScore = false;
            wiggleMe = scores[newHighscoreNum].gameObject.GetComponent<RectTransform>();
            //print("Wiggle This: " + wiggleMe.name);
        }
    }

    void SetHighScores(int i)
    {
        if(currScore > PlayerPrefs.GetInt("HighScore_"+i.ToString()))
        {
            //PlayerPrefs.SetInt("HighScore_"+i.ToString(), currScore);
            int j = 5;
            while(j != i)
            {
                int prevScore = PlayerPrefs.GetInt("HighScore_"+(j-1).ToString());
                PlayerPrefs.SetInt("HighScore_"+j.ToString(), prevScore);
                j--;
            }
            PlayerPrefs.SetInt("HighScore_"+i.ToString(), currScore);
            setNewHighScore = true;
            newHighscoreNum = i-1;
            return;
        }
        else
        {
            if(i == 5)
            {
                return;
            }
            else
            {
                i++;
                SetHighScores(i);
            } 
        }  
    }
}
