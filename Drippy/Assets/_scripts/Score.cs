using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public static int _score = 0;
    private Text myText;


    void Awake()
    {
        //should only run on initial boot up of game
        for(int i = 1; i <= 5; i++)
        {
            if(!PlayerPrefs.HasKey("HighScore_"+i.ToString()))
            {
                PlayerPrefs.SetInt("HighScore_"+i.ToString(),0);
                print("Score Set "+i.ToString()+ ": " + PlayerPrefs.GetInt("HighScore_"+i.ToString()));
            }
        }
    }
    void Start()
    {
        myText = this.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        myText.text = "Score: " + _score.ToString() + "m";

        if(Application.isEditor && Input.GetKeyUp(KeyCode.Q))
        {
            for(int i = 1; i <= 5; i++)
            {
                PlayerPrefs.SetInt("HighScore_"+i.ToString(),0);
                print("RESET SCORES");
            }
        }
    }
}
