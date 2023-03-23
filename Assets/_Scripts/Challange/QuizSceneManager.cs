using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuizSceneManager : MonoBehaviour
{
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
        QuizData.CATEGORY = "All In One";
        SceneManager.LoadScene("9_AllInOne");
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
        QuizData.DIFFICULTY = "Easy";
        SceneManager.LoadScene("9_AllInOne");
    }

    public void Average()
    {
        QuizData.DIFFICULTY = "Average";
        SceneManager.LoadScene("9_AllInOne");
    }

    public void Hard()
    {
        QuizData.DIFFICULTY = "Hard";
        SceneManager.LoadScene("9_AllInOne");
    }

}
