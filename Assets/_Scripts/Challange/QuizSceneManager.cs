using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuizSceneManager : MonoBehaviour
{
    public void toMainMenu()
    {
        SceneManager.LoadScene("0_MainMenu");
    }

    public void Easy()
    {
        // use if else condition to check the category selected
        // SceneManager.LoadScene("New Scene");
    }

    public void Average()
    {
        // use if else condition to check the category selected
        // SceneManager.LoadScene("New Scene");
    }

    public void Hard()
    {
        // use if else condition to check the category selected
        // SceneManager.LoadScene("New Scene");
    }
}
