using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingSceneManagement : MonoBehaviour
{
    public void toMainMenu()
    {
        StartCoroutine(DelaySceneLoad(1));
    }
    public void toQuizMenu()
    {
        StartCoroutine(DelaySceneLoad(2));
    }
    IEnumerator DelaySceneLoad(int scene)
    {
        yield return new WaitForSeconds(0.2f); // Wait 1 seconds
        switch (scene) {
            case 1:
                SceneManager.LoadScene("0_MainMenu");
                break;
            case 2:
                SceneManager.LoadScene("4_Challenge");
                break;
        }
        
    }
}
