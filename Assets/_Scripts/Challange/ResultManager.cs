using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultManager : MonoBehaviour
{
    public void Home()
    {
        SceneManager.LoadScene("4_Challenge");
    }

    public void NextLevel()
    {
        if (QuizData.DIFFICULTY == "Hard") 
        {
            Home();
        }

        if (QuizData.DIFFICULTY == "Average") 
        {
            QuizData.DIFFICULTY = "Hard";
            Retry();
        }

        if (QuizData.DIFFICULTY == "Easy") 
        {
            QuizData.DIFFICULTY = "Average";
            Retry();
        }

    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
