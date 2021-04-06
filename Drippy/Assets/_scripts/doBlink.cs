using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doBlink : MonoBehaviour
{
    public Animator myAnim;

    void Start()
    {
        StartCoroutine(blinkTimer());
    }


    public IEnumerator blinkTimer()
    {
        float blink = Random.Range(1,5);
        
        yield return new WaitForSeconds(blink);
        //Debug.Log("blink");
        myAnim.SetTrigger("doBlink");
        StartCoroutine(blinkTimer());
    }
}
