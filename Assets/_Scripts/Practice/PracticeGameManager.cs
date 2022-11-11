using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PracticeGameManager : MonoBehaviour
{
    public void Home()
    {
        Debug.Log("HOME SELECTED");
        StartCoroutine(DelaySceneLoad());
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Next()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }

    IEnumerator DelaySceneLoad()
    {
        yield return new WaitForSeconds(0.2f); // Wait 1 seconds
        SceneManager.LoadScene("0_MainMenu");
    }


}
