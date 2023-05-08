using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuGameManager : MonoBehaviour
{

    private int scene;

    public void PracticeEasyGame() {
        scene = 0;
        StartCoroutine(DelaySceneLoad());
    }
    public void PracticeAveGame() {
        scene = 1;
        StartCoroutine(DelaySceneLoad());
    }
    public void PracticeHardGame() {
        scene = 2;
        StartCoroutine(DelaySceneLoad());
    }

    public void ChallangeGame() {
        scene = 3;
        StartCoroutine(DelaySceneLoad());
    }
    IEnumerator DelaySceneLoad() {
        yield return new WaitForSeconds(0.2f);
        switch (scene) {
            case 0:
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                break;
            case 1:
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
                break;
            case 2:
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 3);
                break;
            case 3:
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 4);
                break;
            default:
                Debug.Log("Invalid scene index");
                break;
        }
    }
    public void QuitGame() {
        Application.Quit();
    }
}
