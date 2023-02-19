using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuizResultAnim : MonoBehaviour
{
    [Header("Game Objects")]
    public GameObject overlay; 
    public GameObject backPanel; 
    public GameObject ribbon; 
    public GameObject characters; 
    public GameObject star1; 
    public GameObject star2; 
    public GameObject star3;

    [SerializeField]
    GameObject[] buttons;

    [SerializeField]
    CanvasGroup content;

    [Header("Update Text")]
    public TMP_Text Header;
    public TMP_Text HeaderShadow;
    public TMP_Text Category;
    public TMP_Text CategoryShadow;
    public TMP_Text Difficulty;
    public TMP_Text Score;
    public TMP_Text Total;

    [SerializeField]
    float finRibbonPos;

    [SerializeField]
    bool isStarActive;

    Vector3 initpos_backPanel, initpos_ribbon;
    float delay = 0.7f;

    private void Start()
    {
        overlay.SetActive(true);
        initpos_backPanel = backPanel.transform.position;
        initpos_ribbon = ribbon.transform.position;

        // darken color for star
        Color c = new Color32(96, 96, 96, 255); 

        // Great Job! - if perfect
        Header.text = "Great Job!";
        HeaderShadow.text = "Great Job!";

        // Good Job! - if score is less than the total darken 3rd star
        if ( System.Convert.ToInt32(Score.text) < System.Convert.ToInt32(Total.text) )
        {
            Header.text = "Good Job!";
            HeaderShadow.text = "Good Job!";
            star3.GetComponent<Image>().color = c;
        }

        // Try Again! - if score is less than 60% of the total darken 2nd star
        if ( System.Convert.ToInt32(Score.text) < ( (70 * System.Convert.ToInt32(Total.text)) / 100 ) )
        {
            Header.text = "Try Again!";
            HeaderShadow.text = "Try Again!";
            star2.GetComponent<Image>().color = c;
        }

        // Try Again! - Score 0
        if ( System.Convert.ToInt32(Score.text) == 0 )
        {
            star1.GetComponent<Image>().color = c;
        }

    }

    public void setQuiz(string category, string difficulty)
    {
        Category.SetText(category);
        CategoryShadow.SetText(category);
        Difficulty.SetText(difficulty);
    }

    public void setScore(string score, string total)
    {
        Score.SetText(score);
        Total.SetText(total);
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
