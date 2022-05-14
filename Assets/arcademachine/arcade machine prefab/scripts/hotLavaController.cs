using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class hotLavaController : MonoBehaviour
{
    public GameObject game;
    public GameObject machine;
    public GameObject gameController;
    public GameObject explosive;
    public GameObject thrower;

    public float minTime = 1f;
    public float maxTime = 2f;


    private bool spawning = false;
    private bool start = false;

    private Vector3 spawnLoc;

    private GameObject temp;
    private CharacterController explosiveController;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(start)
        {
            spawn();
            //Invoke(nameof(spawn), Random.Range(minTime, maxTime));
            start = false;
        }
    }

    private void spawnToggle()
    {
        if(spawning)
        {
            start = false;
        }
        else
        {
            start = true;
        }
        spawning = !spawning;
    }

    private void spawn()
    {
        if (spawning)
        {


            Debug.Log("spawned");

            spawnLoc = new Vector3(thrower.transform.position.x + Random.Range(-0.2f, 0.3f), thrower.transform.position.y, thrower.transform.position.z + 0.02f);

            temp = Instantiate(explosive, spawnLoc, Quaternion.Euler(new Vector3(Random.Range(0, 359), Random.Range(0, 359), Random.Range(0, 359))));
            temp.transform.SetParent(thrower.transform);
            temp.AddComponent<Rigidbody>();
            temp.layer = 10;
            /*temp.AddComponent<CharacterController>();
            explosiveController = temp.GetComponent<CharacterController>();
            explosiveController.Move(new Vector3(0, -0.2f * Time.fixedDeltaTime, 0));*/
            temp.transform.localScale = explosive.transform.localScale;
            Invoke(nameof(spawn), Random.Range(minTime, maxTime));


        }
    }

}