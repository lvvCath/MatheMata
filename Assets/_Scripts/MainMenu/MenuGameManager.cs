using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuGameManager : MonoBehaviour
{

    private int scene;

    public void PracticeEasyGame() 
    {
        scene = 0;
        StartCoroutine(DelaySceneLoad());
    }
    public void PracticeAveGame()
    {
        scene = 1;
        StartCoroutine(DelaySceneLoad());
    }
    public void PracticeHardGame()
    {
        scene = 2;
        StartCoroutine(DelaySceneLoad());
    }

    public void ChallangeGame()
    {
        scene = 3;
        StartCoroutine(DelaySceneLoad());
    }

    public void Settings()
    {
        Debug.Log("SETTINGS SELECTED");
    }

    public void QuitGame() {
        Application.Quit();
    }

    IEnumerator DelaySceneLoad()
    {
        yield return new WaitForSeconds(0.2f); // Wait 1 seconds
        if(scene == 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        if (scene == 1)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
        }
        if (scene == 2)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 3);
        }
        if (scene == 3)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 4);
        }

    }

    

}
