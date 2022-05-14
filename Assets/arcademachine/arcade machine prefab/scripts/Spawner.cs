using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject explosive;
    public float minTime = 1f;
    public float maxTime = 2f;

    public void Start()
    {
        spawn();
    }

    public void spawn()
    {
        Instantiate(explosive, explosive.transform.localPosition, Quaternion.identity);
        Invoke(nameof(spawn), Random.Range(minTime, maxTime));
    }
}
