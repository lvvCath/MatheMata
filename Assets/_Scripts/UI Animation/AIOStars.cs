using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIOStars : MonoBehaviour
{
    public GameObject[] star;
    private void OnEnable()
    {
        int scoreValue = PlayerPrefs.GetInt("All In One  Score", 0);
        int totalValue = PlayerPrefs.GetInt("All In One  Total", 0);

        Color c = new Color32(96, 96, 96, 255); 
        // if score is less than the total darken 3rd star
        if (scoreValue < totalValue)
        {
            star[2].GetComponent<Image>().color = c;
        }
        // if score is less than 60% of the total darken 2nd star
        if (scoreValue < (0.7f * totalValue))
        {
            star[1].GetComponent<Image>().color = c;
        }
        // Score 0
        if (scoreValue == 0)
        {
            star[0].GetComponent<Image>().color = c;
            star[1].GetComponent<Image>().color = c;
            star[2].GetComponent<Image>().color = c;
        }

        star[0].transform.localScale = new Vector3(0f, 0f, 0f);
        star[1].transform.localScale = new Vector3(0f, 0f, 0f);
        star[2].transform.localScale = new Vector3(0f, 0f, 0f);

        LeanTween.scale(star[2], new Vector3(1f, 1f, 1f), 2f).setEase(LeanTweenType.easeOutElastic);
        LeanTween.scale(star[0], new Vector3(1f, 1f, 1f), 2f).setDelay(.1f).setEase(LeanTweenType.easeOutElastic);
        LeanTween.scale(star[1], new Vector3(1f, 1f, 1f), 2f).setDelay(.2f).setEase(LeanTweenType.easeOutElastic);

    }
}
