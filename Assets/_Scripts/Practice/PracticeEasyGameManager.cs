using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PracticeEasyGameManager : MonoBehaviour
{
    public void Home()
    {
        Debug.Log("HOME SELECTED");
        StartCoroutine(DelaySceneLoad());
    }
    IEnumerator DelaySceneLoad()
    {
        yield return new WaitForSeconds(0.2f); // Wait 1 seconds
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);

    }
}
