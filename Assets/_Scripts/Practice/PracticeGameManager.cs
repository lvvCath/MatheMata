using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PracticeGameManager : MonoBehaviour {
    // Method for returning to the home/main menu
    public void Home() {
        StartCoroutine(DelaySceneLoad());
    }
    // Method for retrying the current scene
    public void Retry() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    // Method for loading the next scene in the build order
    public void Next() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
    // Coroutine to delay scene loading
    IEnumerator DelaySceneLoad() {
        yield return new WaitForSeconds(0.2f); // Wait 1 seconds
        SceneManager.LoadScene("0_MainMenu");
    }
}
