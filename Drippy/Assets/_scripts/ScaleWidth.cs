using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleWidth : MonoBehaviour {

    
    //[ExecuteInEditMode]
    void Start()
    {
        float height = Camera.main.orthographicSize * 2;
        float width = height * Screen.width / Screen.height; // basically height * screen aspect ratio

        this.transform.localScale = new Vector3(width/5.8f, height/10, 0);
    }
}
