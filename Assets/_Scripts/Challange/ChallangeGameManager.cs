using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChallangeGameManager : MonoBehaviour
{
    public void Back()
    {
        StartCoroutine(DelaySceneLoad());
    }
    IEnumerator DelaySceneLoad()
    {
        yield return new WaitForSeconds(0.2f); // Wait 1 seconds
        SceneManager.LoadScene("4_Challange");

    }
    
    public void Home()
    {
        SceneManager.LoadScene("0_MainMenu");
    }

    public void Easy()
    {
        SceneManager.LoadScene("New Scene");
    }
}
