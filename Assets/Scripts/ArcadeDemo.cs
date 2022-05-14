using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcadeDemo : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            MazeController.puzzleCompleted();
            Destroy(gameObject);
        }
    }
}
