using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ScoreSummaryManager : MonoBehaviour {
    // Public Variables
    [Header("Stars")]
    public GameObject[] StarLength;
    public GameObject[] StarMass;
    public GameObject[] StarCapacity;
    [Header("Score")] 
    public TMP_Text[] ScoreLength_TMP;
    public TMP_Text[] ScoreMass_TMP;
    public TMP_Text[] ScoreCapacity_TMP;
    // Private Variables
    // Define arrays for categories, difficulties, score text objects, total score, and total items
    string[] categories = { "Length", "Mass", "Capacity" };
    string[] difficulties = { "Easy", "Average", "Hard" };
    int[] TotalScore;
    int[] TotalItems;
    // Start is called before the first frame update
    void Start() {
        for (int i = 0; i < categories.Length; i++) {
            switch (i) {
                case 0: // Length
                    DisplayScore(i, difficulties, ScoreLength_TMP, StarLength);
                    break;
                case 1: // Mass
                    DisplayScore(i, difficulties, ScoreMass_TMP, StarMass);
                    break;
                case 2: // Capacity
                    DisplayScore(i, difficulties, ScoreCapacity_TMP, StarCapacity);
                    break;
            }
        }
    }
    // Displays the score for a specific category
    void DisplayScore(int categoryIndex, string[] difficulties, TMP_Text[] scoreTexts, GameObject[] stars) {
        TotalScore = new int[categories.Length];
        TotalItems = new int[categories.Length];
        int totalScore = 0;
        int totalItems = 0;
        Color c = new Color32(96, 96, 96, 255);
        for (int j = 0; j < difficulties.Length; j++) {
            string scoreKey = categories[categoryIndex] + " " + difficulties[j] + " Score";
            string totalKey = categories[categoryIndex] + " " + difficulties[j] + " Total";
            int scoreValue = PlayerPrefs.GetInt(scoreKey, 0);
            int totalValue = PlayerPrefs.GetInt(totalKey, 10);
            scoreTexts[j].SetText(scoreValue + " / " + totalValue);
            totalScore += scoreValue;
            totalItems += totalValue;
        }
        TotalScore[categoryIndex] = totalScore;
        TotalItems[categoryIndex] = totalItems;
        // if score is less than the total darken 3rd star
        if (totalScore < totalItems) {
            stars[2].GetComponent<Image>().color = c;
        }
        // if score is less than 70% of the total darken 2nd star
        if (totalScore < (0.7f * totalItems)) {
            stars[1].GetComponent<Image>().color = c;
        }
        // Score 0
        if (totalScore == 0) {
            stars[0].GetComponent<Image>().color = c;
        }
    }
    // Triggered when the object becomes enabled
    private void OnEnable() {
        StarTween(StarLength[0], StarLength[1], StarLength[2]);
        StarTween(StarMass[0], StarMass[1], StarMass[2]);
        StarTween(StarCapacity[0], StarCapacity[1], StarCapacity[2]);
    }
    // Animates the stars using LeanTween library
    void StarTween(GameObject star1, GameObject star2, GameObject star3) {
        star1.transform.localScale = new Vector3(0f, 0f, 0f);
        star2.transform.localScale = new Vector3(0f, 0f, 0f);
        star3.transform.localScale = new Vector3(0f, 0f, 0f);
        // Tween scaling for each star
        LeanTween.scale(star2, new Vector3(1f, 1f, 1f), 2f).setEase(LeanTweenType.easeOutElastic);
        LeanTween.scale(star1, new Vector3(1f, 1f, 1f), 2f).setDelay(.1f).setEase(LeanTweenType.easeOutElastic);
        LeanTween.scale(star3, new Vector3(1f, 1f, 1f), 2f).setDelay(.2f).setEase(LeanTweenType.easeOutElastic);
    }
}
