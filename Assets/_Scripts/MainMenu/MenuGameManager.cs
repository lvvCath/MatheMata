using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuGameManager : MonoBehaviour {
    private int scene;
    // Method for starting an easy practice game
    public void PracticeEasyGame() {
        scene = 0;
        StartCoroutine(DelaySceneLoad());
    }
    // Method for starting an average practice game
    public void PracticeAveGame() {
        scene = 1;
        StartCoroutine(DelaySceneLoad());
    }
    // Method for starting a hard practice game
    public void PracticeHardGame() {
        scene = 2;
        StartCoroutine(DelaySceneLoad());
    }
    // Method for starting a challenge mode
    public void ChallangeGame() {
        scene = 3;
        StartCoroutine(DelaySceneLoad());
    }
    // Coroutine to delay scene loading
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
    // Method for quitting the game
    public void QuitGame() {
        Application.Quit();
    }
}
