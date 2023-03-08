using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PracticeQuizManager : MonoBehaviour
{
    [Header("Questions")]
    public List<PracticeQuestionAnswer> QnA;

    [Header("Buttons Choices")]
    public GameObject[] options;

    [Header("Variable Track")]
    public string difficulty;
    public int currentQuestion;
    public int currentQuestionNo;

    [Header("Texts")]
    public TMP_Text QuestionNoTxt;
    public TMP_Text QuestionTxt;
    public TMP_Text CategoryTxt;

    [Header("PopUp Overlay Panel")]
    public GameObject WrongOverlay;
    public GameObject CorrectOverlay;

    [Header("Quiz UI Panel")]
    public GameObject QuizPanelUI;
    public GameObject QuizPanelBG;
    public GameObject HintPanel;
    public GameObject ResultPanel;

    [Header("Hard Mass Scale")]
    public GameObject scale;

    [Header("Audio SFX")]
    public AudioSource correctSFX;
    public AudioSource wrongSFX;

    private AudioSource audioSource;

    private bool AVE_Capacity_isCorrect = false;

    private void Start()
    {
        generateQuestion();
        audioSource = GetComponent<AudioSource>();
    }

    public void GameOver()
    {
        QuizPanelUI.SetActive(false);
        QuizPanelBG.SetActive(false);
        ResultPanel.SetActive(true);
    }

    public void correct()
    {
        CorrectOverlay.SetActive(true);
        correctSFX.Play();
        StartCoroutine(hideUI(CorrectOverlay, 2.0f, "correct"));

    }

    public void wrong()
    {
        WrongOverlay.SetActive(true);
        wrongSFX.Play();
        StartCoroutine(hideUI(WrongOverlay, 2.0f, "wrong"));
    }

    void SetAnswers() 
    {
        for (int i = 0; i < options.Length; i++)
        {
            options[i].GetComponent<AnswerScript>().isCorrect = false;
            options[i].transform.GetChild(0).GetComponent<TMP_Text>().text = QnA[currentQuestion].Answers[i];

            SetActiveOptionBtn(false);

            // Condition for multiple choice type of question
            if (
               (difficulty == "easy" || // applicable for all easy questions
               (difficulty == "hard" && QnA[currentQuestion].Category != "Mass") ||
               (difficulty == "average" && QnA[currentQuestion].Category != "Capacity")) && // Not applied in average capacity questions
                QnA[currentQuestion].CorrectAnswer == i + 1) // checks if answer and button matches > matched button is set to True
            {
                options[i].GetComponent<AnswerScript>().isCorrect = true;
            }

        }
    }

    public void HardMassSubmit()
    {
        options[3].GetComponent<AnswerScript>().isCorrect = scale.GetComponent<ScaleTrigger>().IsEqual();
    }

    public void AveCapacitySubmit()
    {
        AVE_Capacity_isCorrect = AveCapacityAnswer();
        options[3].GetComponent<AnswerScript>().isCorrect = AVE_Capacity_isCorrect;
    }

    bool AveCapacityAnswer()
    {
        bool check = false;

        for (int i = 0; i < QnA[currentQuestion].AVE_CapacitySlots.Length; i++)
        {
            if (Mathf.Round(QnA[currentQuestion].AVE_CapacitySlots[i].transform.position.x) 
            == Mathf.Round(QnA[currentQuestion].AVE_CapacityAnswer[i].transform.position.x))
            {
                check = true;
            }
            else
            {
                check = false;
                break;
            }
        }
        return check;
    }

    void generateQuestion()
    {
        if (QnA.Count > 0)
        {
            currentQuestionNo += 1;
            QuestionNoTxt.text = "Question " + (currentQuestionNo).ToString();
            QuestionTxt.text = QnA[currentQuestion].Question;
            CategoryTxt.text = QnA[currentQuestion].Category;
            SetActiveBackground(true);

            SetAnswers();
        }
        else
        {
            GameOver();
        }
        
    }

    void SetActiveOptionBtn(bool isActive)
    {
        for (int i = 0; i < options.Length; i++)
        {
            if (QnA[currentQuestion].Answers[i] == "NA")
            {
                options[i].SetActive(isActive);
            }
        }
    }
    void SetActiveBackground(bool isActive)
    {
        for (int i = 0; i < QnA[currentQuestion].Background.Length; i++)
        {
            QnA[currentQuestion].Background[i].SetActive(isActive);
        }
    }

    IEnumerator hideUI(GameObject guiParentCanvas, float secondsToWait, string answer)
    {
        yield return new WaitForSeconds(secondsToWait);
        guiParentCanvas.GetComponent<OverlayPanel>().CloseOverlay();

        if (answer == "correct")
        {
            SetActiveOptionBtn(true);
            SetActiveBackground(false);

            QnA.RemoveAt(currentQuestion);

            generateQuestion();
        }

    }

    public void Hint(bool isActive)
    {
        HintPanel.SetActive(isActive);
        QnA[currentQuestion].Hint.SetActive(isActive);
    }



}
