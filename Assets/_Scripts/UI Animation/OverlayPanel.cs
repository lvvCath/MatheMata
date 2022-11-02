using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayPanel : MonoBehaviour
{
    public GameObject box;
    public CanvasGroup background;

    private void OnEnable()
    {
        background.alpha = 0;
        background.LeanAlpha(1, 0.5f);

        box.transform.localScale = new Vector3(0f, 0f, 0f);

        LeanTween.scale(box, new Vector3(1f, 1f, 1f), 1f).setDelay(.5f).setEase(LeanTweenType.easeOutElastic);
    }

    public void CloseOverlay()
    {
        background.LeanAlpha(0, 0.5f);
        box.transform.LeanScale(Vector2.zero, 0.2f).setOnComplete(OnComplete);
    }

    void OnComplete()
    {
        gameObject.SetActive(false);
    }
}