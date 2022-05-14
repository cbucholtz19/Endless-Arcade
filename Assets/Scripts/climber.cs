using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class climber : MonoBehaviour
{
    public GameObject _player;
    public GameObject arcadeConroller;
    public GameObject hotLavaController;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "thrower")
        {
            Debug.Log("win");
            _player.SendMessage("freeze");
            arcadeConroller.SendMessage("win");
            hotLavaController.SendMessage("spawnToggle");
        }
    }
}
