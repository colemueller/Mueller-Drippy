using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public static int _score = 0;
    private Text myText;
    // Start is called before the first frame update
    void Start()
    {
        myText = this.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        myText.text = "Score: " + _score.ToString() + "m";
    }
}
