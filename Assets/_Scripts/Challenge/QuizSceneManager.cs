using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuizSceneManager : MonoBehaviour
{
    public GameObject[] DifficultyButtons;
    public GameObject LockMessagePanel;
    public void toMainMenu()
    {
        SceneManager.LoadScene("0_MainMenu");
    }

    public void toQuizMenu()
    {
        SceneManager.LoadScene("4_Challenge");
    }

    public void toScoreMenu()
    {
        SceneManager.LoadScene("8_Score");
    }

    public void toAllInOne()
    {
        if (!DifficultyButtons[3].GetComponent<LevelLock>().isLocked) {
            QuizData.CATEGORY = "All In One";
            SceneManager.LoadScene("9_AllInOne");
        } else {
            LockMessagePanel.SetActive(true);
        }
    }

    public void setLengthCategory() {
        QuizData.CATEGORY = "Length";
    }

    public void setMassCategory() {
        QuizData.CATEGORY = "Mass";
    }

    public void setCapacityCategory() {
        QuizData.CATEGORY = "Capacity";
    }

    public void Easy()
    {
        if (!DifficultyButtons[0].GetComponent<LevelLock>().isLocked) {
            QuizData.DIFFICULTY = "Easy";
            SceneManager.LoadScene("9_AllInOne");
        } else {
            LockMessagePanel.SetActive(true);
        }
    }

    public void Average()
    {
        if (!DifficultyButtons[1].GetComponent<LevelLock>().isLocked) {
            QuizData.DIFFICULTY = "Average";
            SceneManager.LoadScene("9_AllInOne");
        } else {
            LockMessagePanel.SetActive(true);
        }
    }

    public void Hard()
    {
        if (!DifficultyButtons[2].GetComponent<LevelLock>().isLocked) {
            QuizData.DIFFICULTY = "Hard";
            SceneManager.LoadScene("9_AllInOne");
        } else {
            LockMessagePanel.SetActive(true);
        }
    }

}
