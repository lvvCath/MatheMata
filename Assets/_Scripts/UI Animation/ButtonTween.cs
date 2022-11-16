using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTween : MonoBehaviour
{
    private void OnEnable()
    {
        gameObject.transform.localScale = new Vector3(.9f, .9f, .9f);
        LeanTween.scale(gameObject, new Vector3(1f, 1f, 1f), 1f).setDelay(.2f).setEase(LeanTweenType.easeOutElastic);
    }
}
