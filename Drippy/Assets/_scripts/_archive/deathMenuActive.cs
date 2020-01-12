using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deathMenuActive : MonoBehaviour {

    public GameObject deathMenu;
    public static bool isMenuActive = false;

    void Start()
    {
        isMenuActive = false;
    }

    void Update()
    {
        if (isMenuActive)
        {
            deathMenu.SetActive(true);
        }
        else
        {
            deathMenu.SetActive(false);
        }
    }
}
