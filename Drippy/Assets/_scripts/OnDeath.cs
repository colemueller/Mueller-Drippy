using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDeath : MonoBehaviour
{
    // Start is called before the first frame update
    public void SetUp()
    {
        EventManager.StartListening("death", InvokeDeath);
        this.gameObject.SetActive(false);
    }

    public void InvokeDeath()
    {
        //print("died");
        this.gameObject.SetActive(true);
    }
}
