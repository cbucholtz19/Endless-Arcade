using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameoverCondition
{
    None,
    Fall,
    Time,
    Arcade,
    Next
}

public class GameController
{
	public static GameoverCondition gameoverCondition = GameoverCondition.None;
	public static int level;

    public static void start_game() {
        level = 1;
        SceneManager.LoadScene("Maze");
    }

    public static void gameover(GameoverCondition c) {
        gameoverCondition = c;
        SceneManager.LoadScene("Menu");
    }

    public static void continueGame() {
        level = level+1;
        SceneManager.LoadScene("Maze");
    }
    


}