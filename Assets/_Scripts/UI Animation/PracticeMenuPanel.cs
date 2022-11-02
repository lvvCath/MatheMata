using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PracticeMenuPanel : MonoBehaviour
{
    [SerializeField]
    GameObject backPanel, ribbon, characters, star1, star2, star3;

    [SerializeField]
    float finRibbonPos;

    [SerializeField]
    bool isStarActive;

    [SerializeField]
    GameObject[] buttons;

    [SerializeField]
    CanvasGroup content;

    Vector3 initpos_backPanel, initpos_ribbon;
    float delay = 0.7f;

    private void Start()
    {
        initpos_backPanel = backPanel.transform.position;
        initpos_ribbon = ribbon.transform.position;
    }

    public void OnEnable()
    {
        content.alpha = 0;
        characters.transform.localScale = new Vector3(0f, 0f, 0f);
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].transform.localScale = new Vector3(0f, 0f, 0f);
        }

        ribbon.transform.localScale = new Vector3(0f, 0f, 0f);
        ribbon.transform.LeanScale(Vector2.one, 0.2f).setDelay(.5f).setOnComplete(OnComplete);
        LeanTween.moveLocal(ribbon, new Vector3(0f, finRibbonPos, 1f), 0.7f).setDelay(1f).setEase(LeanTweenType.easeInOutCubic);

        // For Practice Result Panel
        if (isStarActive)
        {
            star1.transform.localScale = new Vector3(0f, 0f, 0f);
            star2.transform.localScale = new Vector3(0f, 0f, 0f);
            star3.transform.localScale = new Vector3(0f, 0f, 0f);
            LeanTween.scale(ribbon, new Vector3(1f, 1f, 1f), 2f).setDelay(1.7f).setEase(LeanTweenType.easeInOutCubic).setOnComplete(StarsAnim);
        }
        else
        {
            LeanTween.scale(ribbon, new Vector3(1f, 1f, 1f), 2f).setDelay(1.7f).setEase(LeanTweenType.easeInOutCubic);
        }
        

    }

    void OnComplete()
    {
        LeanTween.moveLocal(backPanel, new Vector3(0f, -118f, 0f), 0.7f).setDelay(.5f).setEase(LeanTweenType.easeOutCirc);

        for (int i=0; i < buttons.Length; i++)
        {
            delay += 0.1f;
            LeanTween.scale(buttons[i], new Vector3(1f, 1f, 1f), 2f).setDelay(delay).setEase(LeanTweenType.easeOutElastic);
        }

        content.LeanAlpha(1, 0.5f).setDelay(1f);
        LeanTween.scale(characters, new Vector3(1f, 1f, 1f), 2f).setDelay(1.1f).setEase(LeanTweenType.easeOutElastic);
    }

    void StarsAnim()
    {
        LeanTween.scale(star2, new Vector3(1f, 1f, 1f), 2f).setEase(LeanTweenType.easeOutElastic);
        LeanTween.scale(star1, new Vector3(1f, 1f, 1f), 2f).setDelay(.1f).setEase(LeanTweenType.easeOutElastic);
        LeanTween.scale(star3, new Vector3(1f, 1f, 1f), 2f).setDelay(.2f).setEase(LeanTweenType.easeOutElastic);

    }

    public void exit()
    {
        backPanel.transform.position = initpos_backPanel;
        ribbon.transform.position = initpos_ribbon;
        for (int i = 0; i < buttons.Length; i++)
        {
            LeanTween.scale(buttons[i], new Vector3(1f, 1f, 1f), 0f);
        }

        gameObject.SetActive(false);
    }
}
