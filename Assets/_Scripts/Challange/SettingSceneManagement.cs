using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingSceneManagement : MonoBehaviour
{
    public void toQuizMenu()
    {
        StartCoroutine(DelaySceneLoad());
    }
    IEnumerator DelaySceneLoad()
    {
        yield return new WaitForSeconds(0.2f); // Wait 1 seconds
        SceneManager.LoadScene("4_Challenge");
    }
}
