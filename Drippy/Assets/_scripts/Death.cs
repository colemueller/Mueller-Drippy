using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour {

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            //print("should die...");
            collision.gameObject.SetActive(false);
            EventManager.TriggerEvent("death");

        }
    }
}
