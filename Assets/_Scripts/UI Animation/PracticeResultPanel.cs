using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PracticeResultPanel : MonoBehaviour
{
    [SerializeField]
    GameObject backPanel, ribbon, homeBTN, replayBTN, nextBTN,
    star1, star2, star3, girl, boy;
    [SerializeField]
    CanvasGroup content;

    public void OnEnable()
    {
        LeanTween.scale(ribbon, new Vector3(1.5f, 1.5f, 1.5f), 2f).setDelay(.5f).setEase(LeanTweenType.easeOutElastic).setOnComplete(LevelComplete);
        /*ribbon.transform.LeanScale(Vector2.one, 0.5f).setDelay(.5f);*/
        LeanTween.moveLocal(ribbon, new Vector3(9.53f, 381f, 1f), 0.7f).setDelay(1f).setEase(LeanTweenType.easeInOutCubic).setOnComplete(LevelComplete);
        LeanTween.scale(ribbon, new Vector3(1f, 1f, 1f), 2f).setDelay(1.7f).setEase(LeanTweenType.easeInOutCubic).setOnComplete(StarsAnim);

    }

    void LevelComplete()
    {
        LeanTween.moveLocal(backPanel, new Vector3(0f, 0f, 0f), 0.7f).setDelay(.5f).setEase(LeanTweenType.easeOutCirc);
        LeanTween.scale(homeBTN, new Vector3(1f, 1f, 1f), 2f).setDelay(.8f).setEase(LeanTweenType.easeOutElastic);
        LeanTween.scale(replayBTN, new Vector3(1f, 1f, 1f), 2f).setDelay(.9f).setEase(LeanTweenType.easeOutElastic);
        LeanTween.scale(nextBTN, new Vector3(1f, 1f, 1f), 2f).setDelay(.9f).setEase(LeanTweenType.easeOutElastic);
        /*LeanTween.alpha(content.GetComponent<RectTransform>(), 1f, .5f).setDelay(1f);*/
        content.alpha = 0;
        content.LeanAlpha(1, 0.5f).setDelay(1f);
        LeanTween.scale(girl, new Vector3(1f, 1f, 1f), 2f).setDelay(1.1f).setEase(LeanTweenType.easeOutElastic);
        LeanTween.scale(boy, new Vector3(1f, 1f, 1f), 2f).setDelay(1.2f).setEase(LeanTweenType.easeOutElastic);
    }


    void StarsAnim()
    {
        LeanTween.scale(star2, new Vector3(1f, 1f, 1f), 2f).setEase(LeanTweenType.easeOutElastic);
        LeanTween.scale(star1, new Vector3(1f, 1f, 1f), 2f).setDelay(.1f).setEase(LeanTweenType.easeOutElastic);
        LeanTween.scale(star3, new Vector3(1f, 1f, 1f), 2f).setDelay(.2f).setEase(LeanTweenType.easeOutElastic);

    }
}
