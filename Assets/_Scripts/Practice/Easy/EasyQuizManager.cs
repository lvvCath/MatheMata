using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EasyQuizManager : MonoBehaviour
{
    public List<PracticeQuestionAnswer> QnA;
    public GameObject[] options;
    public int currentQuestion, currentQuestionNo;
    public TMP_Text QuestionNoTxt, QuestionTxt, CategoryTxt;

    public GameObject CorrectOverlay, WrongOverlay;
    public GameObject QuizPanelUI, QuizPanel, ResultPanel;

    private void Start()
    {
        generateQuestion();
    }

    public void GameOver()
    {
        QuizPanelUI.SetActive(false);
        QuizPanel.SetActive(false);
        ResultPanel.SetActive(true);
    }

    public void correct()
    {
        CorrectOverlay.SetActive(true);
        StartCoroutine(hideUI(CorrectOverlay, 2.0f, "correct"));

    }

    public void wrong()
    {
        WrongOverlay.SetActive(true);
        StartCoroutine(hideUI(WrongOverlay, 2.0f, "wrong"));
    }

    void SetAnswers() 
    {
        for (int i = 0; i < options.Length; i++)
        {
            options[i].GetComponent<AnswerScript>().isCorrect = false;
            options[i].transform.GetChild(0).GetComponent<TMP_Text>().text = QnA[currentQuestion].Answers[i];

            SetActiveOptionBtn(false);

            if (QnA[currentQuestion].CorrectAnswer == i + 1) 
            {
                options[i].GetComponent<AnswerScript>().isCorrect = true;
            }
        }
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



}
