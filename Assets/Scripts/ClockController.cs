using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockController : MonoBehaviour
{

    float clockRotation;
    int CLOCK_ROTATION_SPEED = 30;

    // Start is called before the first frame update
    void Start()
    {
        clockRotation = Random.Range(0f,360f);
    }

    // Update is called once per frame
    void Update()
    {
        clockRotation += CLOCK_ROTATION_SPEED * Time.deltaTime;
        if (clockRotation>360) {
            clockRotation = 0;
        }
        gameObject.transform.localEulerAngles = new Vector3(0,clockRotation,0);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            MazeController.clockPickup();
            Destroy(gameObject);
        }
    }
}
