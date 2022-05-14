using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Menu : MonoBehaviour
{

    public GameObject mainMenu;
    public GameObject helpMenu;
    public GameObject gameoverMenu;
    public GameObject highscoreMenu;
    public GameObject nextlevelMenu;
    public GameObject inputField;
    public TMP_Text gameoverConditionText;
    public TMP_Text scoreText;

    public TMP_Text highscore1Text;
    public TMP_Text highscore2Text;
    public TMP_Text highscore3Text;

    void Start() {
        Cursor.visible = true;
        helpMenu.SetActive(false);
        updateHighscores();
        highscoreMenu.SetActive(false);
        if (GameController.gameoverCondition == GameoverCondition.None) { // Start of game
            mainMenu.SetActive(true);
            gameoverMenu.SetActive(false);
        }
        else if (GameController.gameoverCondition == GameoverCondition.Next) {
            mainMenu.SetActive(false);
            gameoverMenu.SetActive(false);
            nextlevelMenu.SetActive(true);
        }
        else { // Restart after losing
            if (GameController.gameoverCondition == GameoverCondition.Fall) {
                gameoverConditionText.text = "You fell out of the world.";
            }
            else if (GameController.gameoverCondition == GameoverCondition.Time) {
                gameoverConditionText.text = "You ran out of time.";
            }
            else if (GameController.gameoverCondition == GameoverCondition.Time) {
                gameoverConditionText.text = "You got exploded!";
            }
            scoreText.text = "Your score: " + (GameController.level-1).ToString();
            mainMenu.SetActive(false);
            gameoverMenu.SetActive(true);
        }
    }

    public void play() {
        GameController.start_game();
    }

    public void quit() {
        Application.Quit();
    }

    public void nextLevel() {
        GameController.continueGame();
    }

    public void submitscore() {
        string name = inputField.GetComponent<TMP_InputField>().text;
        if (name == "") {
            name = "Anonymous";
        }
        int score = GameController.level-1;
        if ( score > PlayerPrefs.GetInt("Highscore1",-1) ) {
            PlayerPrefs.SetString("Highscore3Name", PlayerPrefs.GetString("Highscore2Name","None"));
            PlayerPrefs.SetInt("Highscore3", PlayerPrefs.GetInt("Highscore2",-1));
            PlayerPrefs.SetString("Highscore2Name", PlayerPrefs.GetString("Highscore1Name","None"));
            PlayerPrefs.SetInt("Highscore2", PlayerPrefs.GetInt("Highscore1",-1));
            PlayerPrefs.SetString("Highscore1Name", name);
            PlayerPrefs.SetInt("Highscore1",score);
        }
        else if ( score > PlayerPrefs.GetInt("Highscore2",-1) ) {
            PlayerPrefs.SetString("Highscore3Name", PlayerPrefs.GetString("Highscore2Name","None"));
            PlayerPrefs.SetInt("Highscore3", PlayerPrefs.GetInt("Highscore2",-1));
            PlayerPrefs.SetString("Highscore2Name", name);
            PlayerPrefs.SetInt("Highscore2",score);
        }
        else if ( score > PlayerPrefs.GetInt("Highscore3",-1) ) {
            PlayerPrefs.SetString("Highscore3Name", name);
            PlayerPrefs.SetInt("Highscore3",score);
        }
        updateHighscores();
    }

    private void updateHighscores() {
        highscore1Text.text = PlayerPrefs.GetString("Highscore1Name","None") +" "+ PlayerPrefs.GetInt("Highscore1",-1);
        highscore2Text.text = PlayerPrefs.GetString("Highscore2Name","None") +" "+ PlayerPrefs.GetInt("Highscore2",-1);
        highscore3Text.text = PlayerPrefs.GetString("Highscore3Name","None") +" "+ PlayerPrefs.GetInt("Highscore3",-1);
    }
}
