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
    public GameObject M_EasyCont;
    public GameObject M_AveCont;
    public GameObject M_HardCont;
    public GameObject M_OptionCont;
    public GameObject C_EasyCont;
    public GameObject C_AveCont;
    public GameObject C_HardCont;


    [Header("Quiz Manager")]
    public GameObject QUIZMANAGER;

    // private variables
    private int score;
    private float timer;
    private bool stopTimer;

     private void Start()
    {
        quizTopUI.Category.text = QuizData.CATEGORY;
        CATEGORY = QuizData.CATEGORY;
        DIFFICULTY = QuizData.DIFFICULTY;

        ResultPanel.GetComponent<QuizResultAnim>().setQuiz("All In One", ""); 

        if (CATEGORY == "Length") {
            randCategory = 1;
        } else if (CATEGORY == "Mass") {
            randCategory = 2;
        } else { // Capacity
            randCategory = 3;
        }

        if (DIFFICULTY == "Easy") {
            randDifficulty = 1;
        } else if (DIFFICULTY == "Average") {
            randDifficulty = 2;
        } else { // Hard
            randDifficulty = 3;
        }

        if (CATEGORY != "All In One") {
            ResultPanel.GetComponent<QuizResultAnim>().setQuiz(CATEGORY, DIFFICULTY);
            quizTopUI.Difiiculty.text = DIFFICULTY;
        } else {
            ResultPanel.GetComponent<QuizResultAnim>().setQuiz("All In One", ""); 
            timeLimit = 60;
        }

        QUIZMANAGER.GetComponent<AIOLength>().callStart();
        QUIZMANAGER.GetComponent<AIOMass>().callStart();
        QUIZMANAGER.GetComponent<AIOCapacity>().callStart();
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
            HideContainers();
            quizTopUI.QuestionNo.text = "Question " + (currentQuestionNo+1).ToString();

            if (CATEGORY == "All In One") {
                randCategory = Random.Range(1, 4); // (1) Length, (2) Mass, (3) Capacity
                randDifficulty = Random.Range(1, 4);  // (1) Easy, (2) Average, (3) Hard
            } 
            
            switch (randCategory) {
                case 1:
                    if (CATEGORY == "All In One") {
                        quizTopUI.Difiiculty.text = "Length";
                    }
                    switch (randDifficulty) {
                        case 1:
                            DIFFICULTY = "Easy";
                            timeLimit = 90;
                            L_EasyAveCont.SetActive(true);
                            QUIZMANAGER.GetComponent<AIOLength>().L_EA_Question(DIFFICULTY);
                            QUIZMANAGER.GetComponent<AIOLength>().L_EA_SetAnswers();
                            break;
                        case 2:
                            DIFFICULTY = "Average";
                            timeLimit = 60;
                            L_EasyAveCont.SetActive(true);
                            QUIZMANAGER.GetComponent<AIOLength>().L_EA_Question(DIFFICULTY);
                            QUIZMANAGER.GetComponent<AIOLength>().L_EA_SetAnswers();
                            break;
                        case 3:
                            DIFFICULTY = "Hard";
                            timeLimit = 30;
                            L_HardCont.SetActive(true);
                            QUIZMANAGER.GetComponent<AIOLength>().L_H_Question(DIFFICULTY, currentQuestionNo);
                            QUIZMANAGER.GetComponent<AIOLength>().L_H_SetAnswers(currentQuestionNo);
                            break;
                        default:
                            Debug.LogError("Invalid difficulty level: " + randDifficulty);
                            break;
                    }
                    break;
                case 2:
                    if (CATEGORY == "All In One") {
                        quizTopUI.Difiiculty.text = "Mass";
                    }
                    switch (randDifficulty) {
                        case 1:
                            DIFFICULTY = "Easy";
                            timeLimit = 60;
                            M_EasyCont.SetActive(true);
                            M_OptionCont.SetActive(true);
                            QUIZMANAGER.GetComponent<AIOMass>().EasyQuestion();
                            break;
                        case 2:
                            DIFFICULTY = "Average";
                            timeLimit = 60;
                            M_AveCont.SetActive(true);
                            M_OptionCont.SetActive(true);
                            QUIZMANAGER.GetComponent<AIOMass>().AverageQuestion();
                            break;
                        case 3:
                            DIFFICULTY = "Hard";
                            timeLimit = 90;
                            M_HardCont.SetActive(true);
                            QUIZMANAGER.GetComponent<AIOMass>().HardQuestion();
                            break;
                        default:
                            Debug.LogError("Invalid difficulty level: " + randDifficulty);
                            break;
                    }
                    QUIZMANAGER.GetComponent<AIOMass>().SetAnswers(DIFFICULTY);
                    break;
                case 3:
                    if (CATEGORY == "All In One") {
                        quizTopUI.Difiiculty.text = "Capacity";
                    }
                    switch (randDifficulty) {
                        case 1:
                            DIFFICULTY = "Easy";
                            timeLimit = 30;
                            C_EasyCont.SetActive(true);
                            QUIZMANAGER.GetComponent<AIOCapacity>().EasyQuestion();
                            break;
                        case 2:
                            DIFFICULTY = "Average";
                            timeLimit = 60;
                            C_AveCont.SetActive(true);
                            QUIZMANAGER.GetComponent<AIOCapacity>().AverageQuestion();
                            break;
                        case 3:
                            DIFFICULTY = "Hard";
                            timeLimit = 120;
                            C_HardCont.SetActive(true);
                            QUIZMANAGER.GetComponent<AIOCapacity>().HardQuestion();
                            break;
                        default:
                            Debug.LogError("Invalid difficulty level: " + randDifficulty);
                            break;
                    }
                    QUIZMANAGER.GetComponent<AIOCapacity>().SetAnswers(DIFFICULTY);
                    break;
                default:
                    Debug.LogError("Invalid category: " + randCategory);
                    break;
            }
            stopTimer = false;
            quizTopUI.TimerSlider.maxValue = timeLimit;
            quizTopUI.TimerSlider.value = timeLimit;

            timer = timeLimit;
            
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
        
        StopQuestionAudio();

        quizTopUI.Score.text = (score).ToString() + " / " + total.ToString();
        CorrectOverlay.SetActive(true);
        correctSFX.Play();
        StartCoroutine(nextQuestion(CorrectOverlay, 2.0f, "correct"));
    }

    public void wrong()
    {
        stopTimer = true; // Timer

        StopQuestionAudio();

        WrongOverlay.SetActive(true);
        wrongSFX.Play();
        StartCoroutine(nextQuestion(WrongOverlay, 2.0f, "wrong"));
    }

    private void HideContainers()
    {
        L_EasyAveCont.SetActive(false);
        L_HardCont.SetActive(false);
        M_EasyCont.SetActive(false);
        M_AveCont.SetActive(false);
        M_HardCont.SetActive(false);
        M_OptionCont.SetActive(false);
        C_EasyCont.SetActive(false);
        C_AveCont.SetActive(false);
        C_HardCont.SetActive(false);
    }

    public void PlayQuestion()
    {
        switch (randCategory) {
            case 1:
                switch (randDifficulty) {
                    case 1:
                        DIFFICULTY = "Easy";
                        QUIZMANAGER.GetComponent<AIOLength>().ToggleQuestionAudio(DIFFICULTY, currentQuestionNo);
                        break;
                    case 2:
                        DIFFICULTY = "Average";
                        QUIZMANAGER.GetComponent<AIOLength>().ToggleQuestionAudio(DIFFICULTY, currentQuestionNo);
                        break;
                    case 3:
                        DIFFICULTY = "Hard";
                        QUIZMANAGER.GetComponent<AIOLength>().ToggleQuestionAudio(DIFFICULTY, currentQuestionNo);
                        break;
                    default:
                        Debug.LogError("Invalid difficulty level: " + randDifficulty);
                        break;
                }
                break;
            case 2:
                switch (randDifficulty) {
                    case 1:
                        DIFFICULTY = "Easy";
                        QUIZMANAGER.GetComponent<AIOMass>().ToggleQuestionAudio(DIFFICULTY);
                        break;
                    case 2:
                        DIFFICULTY = "Average";
                        QUIZMANAGER.GetComponent<AIOMass>().ToggleQuestionAudio(DIFFICULTY);
                        break;
                    case 3:
                        DIFFICULTY = "Hard";
                        QUIZMANAGER.GetComponent<AIOMass>().ToggleQuestionAudio(DIFFICULTY);
                        break;
                    default:
                        Debug.LogError("Invalid difficulty level: " + randDifficulty);
                        break;
                }
                break;
            case 3:
                switch (randDifficulty) {
                    case 1:
                        DIFFICULTY = "Easy";
                        QUIZMANAGER.GetComponent<AIOCapacity>().ToggleQuestionAudio(DIFFICULTY);
                        break;
                    case 2:
                        DIFFICULTY = "Average";
                        QUIZMANAGER.GetComponent<AIOCapacity>().ToggleQuestionAudio(DIFFICULTY);
                        break;
                    case 3:
                        DIFFICULTY = "Hard";
                        QUIZMANAGER.GetComponent<AIOCapacity>().ToggleQuestionAudio(DIFFICULTY);
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
    }

    private void StopQuestionAudio()
    {
        switch (randCategory) {
            case 1:
                QUIZMANAGER.GetComponent<AIOLength>().StopQuestionAudio();
                break;
            case 2:
                QUIZMANAGER.GetComponent<AIOMass>().StopQuestionAudio();
                break;
            case 3:
                QUIZMANAGER.GetComponent<AIOCapacity>().StopQuestionAudio();
                break;
            default:
                Debug.LogError("Invalid category: " + randCategory);
                break;
        }
    }

}