using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PracticeQuizManager : MonoBehaviour {
    // Public Variables
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
    // Private Variables
    private AudioSource audioSource;
    private bool AVE_Capacity_isCorrect = false;
    
    // Starts the script once the scene is running.
    private void Start() {
        generateQuestion();
        audioSource = GetComponent<AudioSource>();
    }
    // Method called when the game is over
    public void GameOver() {
        QuizPanelUI.SetActive(false);
        QuizPanelBG.SetActive(false);
        ResultPanel.SetActive(true);
    }
    // Method called when the answer is correct
    public void correct() {
        CorrectOverlay.SetActive(true);
        correctSFX.Play();
        StartCoroutine(hideUI(CorrectOverlay, 2.0f, "correct"));
    }
    // Method called when the answer is wrong
    public void wrong() {
        WrongOverlay.SetActive(true);
        wrongSFX.Play();
        StartCoroutine(hideUI(WrongOverlay, 2.0f, "wrong"));
    }
    // Method to set the answer choices for the current question
    void SetAnswers() {
        for (int i = 0; i < options.Length; i++) {
            options[i].GetComponent<AnswerScript>().isCorrect = false;
            options[i].transform.GetChild(0).GetComponent<TMP_Text>().text = QnA[currentQuestion].Answers[i];
            SetActiveOptionBtn(false);
            if ((difficulty == "easy" || // applicable for all easy questions
               (difficulty == "hard" && QnA[currentQuestion].Category != "Mass") ||
               // Not applied in average capacity questions
               (difficulty == "average" && QnA[currentQuestion].Category != "Capacity")) && 
               // checks if answer and button matches > matched button is set to True
                QnA[currentQuestion].CorrectAnswer == i + 1) 
            {
                options[i].GetComponent<AnswerScript>().isCorrect = true;
            }
        }
    }
    // Method called when the user submits the answer for level Hard Mass question
    public void HardMassSubmit() {
        options[3].GetComponent<AnswerScript>().isCorrect = scale.GetComponent<ScaleTrigger>().IsEqual();
    }
    // Method called when the user submits the answer for Average Capacity Question
    public void AveCapacitySubmit() {
        AVE_Capacity_isCorrect = AveCapacityAnswer();
        options[3].GetComponent<AnswerScript>().isCorrect = AVE_Capacity_isCorrect;
    }
    // Method called when the user submits the answer for Average Capacity Question
    bool AveCapacityAnswer() {
        bool check = false;
        for (int i = 0; i < QnA[currentQuestion].AVE_CapacitySlots.Length; i++) {
            if (Mathf.Round(QnA[currentQuestion].AVE_CapacitySlots[i].transform.position.x) 
            == Mathf.Round(QnA[currentQuestion].AVE_CapacityAnswer[i].transform.position.x))
            {
                check = true;
            } else {
                check = false;
                break;
            }
        }
        return check;
    }
    // Sets the question to be shown to the player
    void generateQuestion() {
        if (QnA.Count > 0) {
            currentQuestionNo += 1;
            QuestionNoTxt.text = "Question " + (currentQuestionNo).ToString();
            QuestionTxt.text = QnA[currentQuestion].Question;
            CategoryTxt.text = QnA[currentQuestion].Category;
            SetActiveBackground(true); 
            SetAnswers();
        } else {
            GameOver();
        } 
    }
    // Method to set the active state of Option buttons based on the current question
    void SetActiveOptionBtn(bool isActive) {
        for (int i = 0; i < options.Length; i++) {
            if (QnA[currentQuestion].Answers[i] == "NA") {
                options[i].SetActive(isActive);
            }
        }
    }
    // Method to set the active state of background elements based on the current question
    void SetActiveBackground(bool isActive) {
        for (int i = 0; i < QnA[currentQuestion].Background.Length; i++) {
            QnA[currentQuestion].Background[i].SetActive(isActive);
        }
    }
    // Coroutine to hide the UI elements after a certain delay
    IEnumerator hideUI(GameObject guiParentCanvas, float secondsToWait, string answer) {
        yield return new WaitForSeconds(secondsToWait);
        guiParentCanvas.GetComponent<OverlayPanel>().CloseOverlay(); 
        if (answer == "correct") {
            SetActiveOptionBtn(true);
            SetActiveBackground(false); 
            QnA.RemoveAt(currentQuestion); 
            generateQuestion();
        } 
    }
    // Method to show or hide the Hint panel
    public void Hint(bool isActive) {
        HintPanel.SetActive(isActive);
        QnA[currentQuestion].Hint.SetActive(isActive);
    }
}
