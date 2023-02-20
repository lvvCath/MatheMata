using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuizSceneManager : MonoBehaviour
{
    public void toMainMenu()
    {
        SceneManager.LoadScene("0_MainMenu");
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

        if (QuizData.CATEGORY == "Length") 
        {
            SceneManager.LoadScene("5_Length");
        }
        if (QuizData.CATEGORY == "Mass") 
        {
            SceneManager.LoadScene("6_Mass");
        }
        // if (QuizData.CATEGORY == "Capacity") 
        // {
            
        // }
    }

    public void Average()
    {
        QuizData.DIFFICULTY = "Average";

        if (QuizData.CATEGORY == "Length") 
        {
            SceneManager.LoadScene("5_Length");
        }
        if (QuizData.CATEGORY == "Mass") 
        {
            SceneManager.LoadScene("6_Mass");
        }
        // if (QuizData.CATEGORY == "Capacity") 
        // {
            
        // }
    }

    public void Hard()
    {
        QuizData.DIFFICULTY = "Hard";

        if (QuizData.CATEGORY == "Length") 
        {
            SceneManager.LoadScene("5_Length");
        }
        if (QuizData.CATEGORY == "Mass") 
        {
            SceneManager.LoadScene("6_Mass");
        }
        // if (QuizData.CATEGORY == "Capacity") 
        // {
            
        // }
    }


}
