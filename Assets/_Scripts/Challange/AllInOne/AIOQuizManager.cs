using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class AIOQuizManager : MonoBehaviour
{
    [Header("Quiz Info")]
    public string CATEGORY;
    public string DIFFICULTY;
    public int randCategory;
    public int randDifficulty;
    [Header("Quiz Top Panel")]
    public QuizTopUI quizTopUI;
    [Header("Quiz Values")]
    public int currentQuestionNo;
    public int total;
    public float timeLimit;
    [Header("PopUp Overlay Panel")]
    public GameObject WrongOverlay;
    public GameObject CorrectOverlay;
    public GameObject ResultPanel;
    [Header("Audio SFX")]
    public AudioSource correctSFX;
    public AudioSource wrongSFX;

    [Header("Question Content Container")]
    public GameObject L_EasyAveCont;
    public GameObject L_HardCont;

    // private variables
    private int score;
    private float timer;
    private bool stopTimer;

    public GameObject aioLength;

     private void Start()
    {
        quizTopUI.Category.text = "All in One";
        ResultPanel.GetComponent<QuizResultAnim>().setQuiz("All In One", ""); 

        // Timer
        timeLimit = 30;
        stopTimer = false;
        quizTopUI.TimerSlider.maxValue = timeLimit;
        quizTopUI.TimerSlider.value = timeLimit;

        GenerateQuestion();
    }

    private void Update() 
    {
        if(stopTimer == false)
        {
            timer -= Time.deltaTime;
            // Text Timer
            int minutes = Mathf.FloorToInt(timer / 60);
            int seconds = Mathf.FloorToInt(timer % 60);
            quizTopUI.Timer.text = minutes.ToString("00") + ":" + seconds.ToString("00");
            // Slider Timer
            quizTopUI.TimerSlider.value = timer;

            if (timer <= 0)
            {
                GenerateQuestion();
            }
        }
        
    }

    private void GenerateQuestion() 
    {
        if (currentQuestionNo < total)
        {
            L_EasyAveCont.SetActive(false);
            L_HardCont.SetActive(false);

            // Timer
            stopTimer = false;
            timer = timeLimit;

            quizTopUI.QuestionNo.text = "Question " + (currentQuestionNo+1).ToString();

            // int randCategory = Random.Range(1, 4); // (1) Length, (2) Mass, (3) Capacity
            randDifficulty = Random.Range(1, 4);  // (1) Easy, (2) Average, (3) Hard

            // randCategory = 1;
            // randDifficulty = 1;
            switch (randCategory) {
                case 1:
                    quizTopUI.Difiiculty.text = "Length";
                    switch (randDifficulty) {
                        case 1:
                            DIFFICULTY = "Easy";
                            L_EasyAveCont.SetActive(true);
                            aioLength.GetComponent<AIOLength>().L_EA_Question(DIFFICULTY);
                            aioLength.GetComponent<AIOLength>().L_EA_SetAnswers();
                            break;
                        case 2:
                            DIFFICULTY = "Average";
                            L_EasyAveCont.SetActive(true);
                            aioLength.GetComponent<AIOLength>().L_EA_Question(DIFFICULTY);
                            aioLength.GetComponent<AIOLength>().L_EA_SetAnswers();
                            break;
                        case 3:
                            DIFFICULTY = "Hard";
                            L_HardCont.SetActive(true);
                            aioLength.GetComponent<AIOLength>().L_H_Question(DIFFICULTY, currentQuestionNo);
                            aioLength.GetComponent<AIOLength>().L_H_SetAnswers(currentQuestionNo);
                            break;
                        default:
                            Debug.LogError("Invalid difficulty level: " + randDifficulty);
                            break;
                    }
                    break;
                case 2:
                    quizTopUI.Difiiculty.text = "Mass";
                    switch (randDifficulty) {
                        case 1:
                            DIFFICULTY = "Easy";
                            //...
                            break;
                        case 2:
                            DIFFICULTY = "Average";
                            //...
                            break;
                        case 3:
                            DIFFICULTY = "Hard";
                            //...
                            break;
                        default:
                            Debug.LogError("Invalid difficulty level: " + randDifficulty);
                            break;
                    }
                    break;
                case 3:
                    quizTopUI.Difiiculty.text = "Capacity";
                    switch (randDifficulty) {
                        case 1:
                            DIFFICULTY = "Easy";
                            //...
                            break;
                        case 2:
                            DIFFICULTY = "Average";
                            //...
                            break;
                        case 3:
                            DIFFICULTY = "Hard";
                            //...
                            break;
                        default:
                            Debug.LogError("Invalid difficulty level: " + randDifficulty);
                            break;
                    }
                    break;
                default:
                    Debug.LogError("Invalid category: " + randCategory);
                    break;
            }
            
            currentQuestionNo += 1;
        }
        else
        {
            // Timer
            stopTimer = true;
            quizTopUI.Timer.text = "00:00";

            // Display Result Panel
            ResultPanel.SetActive(true);
            ResultPanel.GetComponent<QuizResultAnim>().setScore(score.ToString(), total.ToString());
            
            Debug.Log("GAME OVER");
        }

    }

    IEnumerator nextQuestion(GameObject guiParentCanvas, float secondsToWait, string answer)
    {
        yield return new WaitForSeconds(secondsToWait);
        guiParentCanvas.GetComponent<OverlayPanel>().CloseOverlay();

        GenerateQuestion();

    }

    public void correct()
    {
        score += 1;
        stopTimer = true; // Timer

        quizTopUI.Score.text = (score).ToString() + " / " + total.ToString();
        CorrectOverlay.SetActive(true);
        correctSFX.Play();
        StartCoroutine(nextQuestion(CorrectOverlay, 2.0f, "correct"));
    }

    public void wrong()
    {
        stopTimer = true; // Timer

        WrongOverlay.SetActive(true);
        wrongSFX.Play();
        StartCoroutine(nextQuestion(WrongOverlay, 2.0f, "wrong"));
    }

}