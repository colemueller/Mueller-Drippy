using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generate : MonoBehaviour {

    public GameObject holdPrefab;
    public Transform parentObj;
    public float moveSpeed;
    public float timer;

    public void Start()
    {
        StartCoroutine(spawn(timer));
        GenerateHold();
    }

    public void Update()
    {
        transform.Translate((Vector3.down * moveSpeed) * Time.deltaTime);
    }

    public void GenerateHold()
    {
        float rand = Random.Range(-1.5f, 1.5f);
        //Debug.Log(rand);
        GameObject clone = Instantiate(holdPrefab, new Vector3(rand, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
        clone.transform.SetParent(parentObj);
    }

    IEnumerator spawn(float time)
    {
        yield return new WaitForSeconds(time);

        GenerateHold();

        StartCoroutine(spawn(time));
    }
}
