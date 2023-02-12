using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Splashscreen : MonoBehaviour
{
    [SerializeField]
    GameObject BG, KCLogo, TIPLogo, MatheMataLogo;

    // Start is called before the first frame update
    void Start()
    {

        KCLogo.transform.localScale = new Vector3(0f, .0f, 0f);
        TIPLogo.transform.localScale = new Vector3(0f, 0f, 0f);

        LeanTween.scale(KCLogo, new Vector3(1f, 1f, 1f), 1f).setDelay(.6f).setEase(LeanTweenType.easeOutElastic);
        LeanTween.scale(TIPLogo, new Vector3(1f, 1f, 1f), 1f).setDelay(.6f).setEase(LeanTweenType.easeOutElastic).setOnComplete(AppLogo);

        LeanTween.scale(BG, new Vector3(1.3f, 1.3f, 1.3f), 1.3f).setEase(LeanTweenType.linear).setTime(10);

    }

    void AppLogo() 
    {
        // hide KC and TIP logo
        LeanTween.scale(KCLogo, new Vector3(0f, 0f, 0f), 1f).setDelay(.6f).setEase(LeanTweenType.easeOutElastic);
        LeanTween.scale(TIPLogo, new Vector3(0f, 0f, 0f), 1f).setDelay(.6f).setEase(LeanTweenType.easeOutElastic);

        // set MatheMata Logo
        MatheMataLogo.SetActive(true);
        MatheMataLogo.transform.localScale = new Vector3(0f, .0f, 0f);

        LeanTween.scale(MatheMataLogo, new Vector3(70f, 70f, 70f), 1f).setDelay(.6f).setEase(LeanTweenType.easeOutElastic).setOnComplete(MainMenu);
    }

    void MainMenu()
    {
        StartCoroutine(DelaySceneLoad());
    }

    IEnumerator DelaySceneLoad()
    {
        yield return new WaitForSeconds(2.5f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }
}
