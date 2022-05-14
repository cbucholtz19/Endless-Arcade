using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MazeController : MonoBehaviour
{
    public GameObject player;

    // Timer vars
    public static float timer;
    public TMP_Text timerText;

    // Waypoint vars
    public GameObject waypoint1;
    public GameObject waypoint2;
    public GameObject waypoint3;
    private bool w1i;
    private bool w2i;
    private bool w3i;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        timer = 50f + 10f * GameController.level;
        w1i = false;
        w2i = false;
        w3i = false;
    }

    // Update is called once per frame
    void Update()
    {
        timer = timer - Time.deltaTime;
        timerText.text = ((int)timer+1).ToString();

        // Check for game ending conditions
        if (timer < 0.0f) {
            GameController.gameover(GameoverCondition.Time);
        }
        if (player.transform.localPosition.y < (-50.0f)) {
            GameController.gameover(GameoverCondition.Fall);
        }

        // Check for waypoint input
        if (Input.GetKeyDown("1")) {
            if (!w1i) {
                waypoint1 = Instantiate(waypoint1, new Vector3(player.transform.localPosition.x, 50, player.transform.localPosition.z), Quaternion.identity);
                w1i = true;
            }
            else {
                waypoint1.transform.position = new Vector3(player.transform.localPosition.x, 50, player.transform.localPosition.z);
            }
        }
        else if (Input.GetKeyDown("2")) {
            if (!w2i) {
                waypoint2 = Instantiate(waypoint2, new Vector3(player.transform.localPosition.x, 50, player.transform.localPosition.z), Quaternion.identity);
                w2i = true;
            }
            else {
                waypoint2.transform.position = new Vector3(player.transform.localPosition.x, 50, player.transform.localPosition.z);
            }
        }
        else if (Input.GetKeyDown("3")) {
            if (!w3i) {
                waypoint3 = Instantiate(waypoint3, new Vector3(player.transform.localPosition.x, 50, player.transform.localPosition.z), Quaternion.identity);
                w3i = true;
            }
            else {
                waypoint3.transform.position = new Vector3(player.transform.localPosition.x, 50, player.transform.localPosition.z);
            }
        }
    }

    public static void clockPickup() {
        timer = timer + 10.0f;
    }

    public static void puzzleCompleted() {
        GameController.gameover(GameoverCondition.Next);
    }
}
