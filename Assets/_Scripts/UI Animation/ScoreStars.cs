using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScoreStars : MonoBehaviour
{
    public GameObject[] star;
    string[] categories = { "Length", "Mass", "Capacity" };
    string[] difficulties = { "Easy", "Average", "Hard" };

    int totalScore;
    int totalItems;
    private void OnEnable()
    {
        totalScore = 0;
        totalItems = 0;

        for (int i = 0; i < categories.Length; i++)
        {
            switch (i)
            {
                case 0: // Length
                    DisplayScore(i, difficulties);
                    break;
                case 1: // Mass
                    DisplayScore(i, difficulties);
                    break;
                case 2: // Capacity
                    DisplayScore(i, difficulties);
                    break;
            }

        }

        Color c = new Color32(96, 96, 96, 255); 
        // if score is less than the total darken 3rd star
        if (totalScore < totalItems)
        {
            star[2].GetComponent<Image>().color = c;
        }
        // if score is less than 60% of the total darken 2nd star
        if (totalScore < (0.7f * totalItems))
        {
            star[1].GetComponent<Image>().color = c;
        }
        // Score 0
        if (totalScore == 0)
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

    void DisplayScore(int categoryIndex, string[] difficulties) {
        for (int j = 0; j < difficulties.Length; j++)
        {
            string scoreKey = categories[categoryIndex] + " " + difficulties[j] + " Score";
            string totalKey = categories[categoryIndex] + " " + difficulties[j] + " Total";

            int scoreValue = PlayerPrefs.GetInt(scoreKey, 0);
            int totalValue = PlayerPrefs.GetInt(totalKey, 10);

            totalScore += scoreValue;
            totalItems += totalValue;
        }
    }
}

