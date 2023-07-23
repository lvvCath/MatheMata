using UnityEngine;
using UnityEngine.SceneManagement;

public class QuizSceneManager : MonoBehaviour {
    // Public Variables
    public GameObject[] DifficultyButtons;
    public GameObject LockMessagePanel;
    // Loads the main menu scene
    public void toMainMenu() {
        SceneManager.LoadScene("0_MainMenu");
    }
    // Loads the quiz menu scene
    public void toQuizMenu() {
        SceneManager.LoadScene("4_Challenge");
    }
    // Loads the score menu scene
    public void toScoreMenu() {
        SceneManager.LoadScene("8_Score");
    }
    // Loads the Quiz scene (scene for all the levels in challenge mode) if unlocked
    // otherwise shows lock message
    public void toAllInOne() {
        if (!DifficultyButtons[3].GetComponent<LevelLock>().isLocked) {
            QuizData.CATEGORY = "All In One";
            SceneManager.LoadScene("9_AllInOne");
        } else {
            LockMessagePanel.SetActive(true);
        }
    }
    // Sets the selected challenge category to "Length"
    public void setLengthCategory() {
        QuizData.CATEGORY = "Length";
    }
    // Sets the selected challenge category to "Mass"
    public void setMassCategory() {
        QuizData.CATEGORY = "Mass";
    }
    // Sets the selected challenge category to "Capacity"
    public void setCapacityCategory() {
        QuizData.CATEGORY = "Capacity";
    }
    // Loads Quiz scene for challenge mode levels if level is unlocked and
    // sets the selected challenge difficulty to "Easy"
    // otherwise shows lock message
    public void Easy() {
        if (!DifficultyButtons[0].GetComponent<LevelLock>().isLocked) {
            QuizData.DIFFICULTY = "Easy";
            SceneManager.LoadScene("9_AllInOne");
        } else {
            LockMessagePanel.SetActive(true);
        }
    }
    // Loads Quiz scene for challenge mode levels if level is unlocked and
    // sets the selected challenge difficulty to "Average"
    // otherwise shows lock message
    public void Average() {
        if (!DifficultyButtons[1].GetComponent<LevelLock>().isLocked) {
            QuizData.DIFFICULTY = "Average";
            SceneManager.LoadScene("9_AllInOne");
        } else {
            LockMessagePanel.SetActive(true);
        }
    }
    // Loads Quiz scene for challenge mode levels if level is unlocked and
    // sets the selected challenge difficulty to "Hard"
    // otherwise shows lock message
    public void Hard() {
        if (!DifficultyButtons[2].GetComponent<LevelLock>().isLocked) {
            QuizData.DIFFICULTY = "Hard";
            SceneManager.LoadScene("9_AllInOne");
        } else {
            LockMessagePanel.SetActive(true);
        }
    }
}
