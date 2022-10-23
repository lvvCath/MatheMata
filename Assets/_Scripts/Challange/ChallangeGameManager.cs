using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChallangeGameManager : MonoBehaviour
{
    public void Back()
    {
        Debug.Log("BACK SELECTED");
        StartCoroutine(DelaySceneLoad());
    }
    IEnumerator DelaySceneLoad()
    {
        yield return new WaitForSeconds(0.2f); // Wait 1 seconds
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 4);

    }
}
