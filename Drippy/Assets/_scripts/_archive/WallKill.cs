using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallKill : MonoBehaviour {

    

    private void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //deathMenu = GameObject.FindGameObjectWithTag("deathMenu");
            collision.gameObject.SetActive(false);
            deathMenuActive.isMenuActive = true;
            

        }
    }
}
