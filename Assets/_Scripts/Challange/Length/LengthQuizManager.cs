using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class LengthQuizManager : MonoBehaviour
{
    [Header("Game Objects")]
    public string CATEGORY;
    public string DIFFICULTY;

    [Header("Game Objects")]
    public GameObject[] LongObjects;
    public GameObject[] ShortObjects;

    [Header("Quiz Options")]
    public GameObject[] Options;

    [Header("Game Object Container")]
    public GameObject ParentContainer;
    public GameObject ShortContainer;
    public GameObject LongContainer;

    public QuizTopUI quizTopUI;

    public int currentQuestionNo;
    public float timeLimit;
    private int score;
    private float timer;
    private bool stopTimer;

    private List<int> shortRecord = new List<int>();

    // Easy
    private int correctEasyAns;
    private string shortEasyObjName;

    // Average
    private GameObject missingShortObj;

    [Header("PopUp Overlay Panel")]
    public GameObject WrongOverlay;
    public GameObject CorrectOverlay;

    public GameObject ResultPanel;

    [Header("Audio SFX")]
    public AudioClip correctSFX;
    public AudioClip wrongSFX;

    private AudioSource audioSource;

     private void Start()
    {
        // Set Here the current Text for Question, Category, Difficulty. use condition

        quizTopUI.Category.text = "Length";
        
        // Timer
        stopTimer = false;
        quizTopUI.TimerSlider.maxValue = timeLimit;
        quizTopUI.TimerSlider.value = timeLimit;

        DurstenfeldShuffle(LongObjects);
        GenerateQuestion();
        audioSource = GetComponent<AudioSource>();

        
        if (DIFFICULTY == "Easy") {
            quizTopUI.Difiiculty.text = "Easy";
            ResultPanel.GetComponent<QuizResultAnim>().setQuiz("Length", "Easy"); 
        }

        if (DIFFICULTY == "Average") {
            quizTopUI.Difiiculty.text = "Average";
            ResultPanel.GetComponent<QuizResultAnim>().setQuiz("Length", "Average"); 
        }

        if (DIFFICULTY == "Hard") {
            quizTopUI.Difiiculty.text = "Hard";
            ResultPanel.GetComponent<QuizResultAnim>().setQuiz("Length", "Hard"); 
        }
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
    
    // Shuffle Algorithm
    private void DurstenfeldShuffle<T>(T[] gameObjectArr) 
    {
        int last_index = gameObjectArr.Length - 1;
        while (last_index > 0)
        {
            int rand_index = Random.Range(0, last_index+1); //modify in documentation "+1"
            T temp = gameObjectArr[last_index];
            gameObjectArr[last_index] = gameObjectArr[rand_index];
            gameObjectArr[rand_index] = temp;
            last_index -= 1;
        }
    }
    

    private void GenerateQuestion() 
    {
        if (currentQuestionNo < 10)
        {
            // Timer
            stopTimer = false;
            timer = timeLimit;

            quizTopUI.QuestionNo.text = "Question " + (currentQuestionNo+1).ToString();

            // insert here if else condition to generate question based on selected lvl of difficulty
            EasyQuestion();
            SetAnswers();

            if (DIFFICULTY == "Easy") {
                quizTopUI.Question.text = "How many <color=#ffcb2b>" + shortEasyObjName + "</color> required to equal the length of the <color=#ffcb2b>" + LongObjects[currentQuestionNo].name + "</color>?"; 
            }

            if (DIFFICULTY == "Average") {
                quizTopUI.Question.text = "How many <color=#ffcb2b>missing</color> " + shortEasyObjName + " required to equal the length of the " + LongObjects[currentQuestionNo].name + "?"; 
            }

            if (DIFFICULTY == "Hard") {
                quizTopUI.Question.text = "Question"; 
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
            ResultPanel.GetComponent<QuizResultAnim>().setScore(score.ToString(), LongObjects.Length.ToString());
            
            Debug.Log("GAME OVER");
        }

    }

    public void SetAnswers()
    {
        int[] easyAnswersArr = { correctEasyAns+1, correctEasyAns, correctEasyAns-1 };
        DurstenfeldShuffle(easyAnswersArr);
        
        for (int i = 0; i < Options.Length; i++)
        {
            Options[i].GetComponent<LengthAnswerScript>().isCorrect = false;
            Options[i].transform.GetChild(0).GetComponent<TMP_Text>().text = easyAnswersArr[i].ToString() + " " + shortEasyObjName;

            // Condition for multiple choice type of question
            if (easyAnswersArr[i] == correctEasyAns) // checks if answer and button matches > matched button is set to True
            {
                Options[i].GetComponent<LengthAnswerScript>().isCorrect = true;
            } 

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

        quizTopUI.Score.text = (score).ToString() + " / " + LongObjects.Length.ToString();
        CorrectOverlay.SetActive(true);
        audioSource.PlayOneShot(correctSFX);
        StartCoroutine(nextQuestion(CorrectOverlay, 2.0f, "correct"));
    }

    public void wrong()
    {
        stopTimer = true; // Timer

        WrongOverlay.SetActive(true);
        audioSource.PlayOneShot(wrongSFX);
        StartCoroutine(nextQuestion(WrongOverlay, 2.0f, "wrong"));
    }

// Length Easy and Average Difficulty
    private void EasyQuestion()
    {
        if (ShortContainer.transform.childCount > 0)
        {
            for (var i = ShortContainer.transform.childCount - 1; i >= 0; i--)
            {
                Object.Destroy(ShortContainer.transform.GetChild(i).gameObject);
            }
        }
        if (currentQuestionNo != 0)
        {
            LongObjects[currentQuestionNo-1].SetActive(false);
        }

        // Set Width of Parent Container using the long object width.
        float child_width = LongObjects[currentQuestionNo].GetComponent<RectTransform>().rect.width;
        float parent_height = ParentContainer.GetComponent<RectTransform>().rect.height;

        ParentContainer.GetComponent<RectTransform>().sizeDelta = new Vector2(child_width, parent_height);

        LongObjects[currentQuestionNo].SetActive(true);

        // Short Object
        int currShortObject = Random.Range(0, ShortObjects.Length);
        bool flag = true;
        while (flag)
        {
            currShortObject = Random.Range(0, ShortObjects.Length);
            if (shortRecord.Contains(currShortObject) == false) // checks if short object was already used in question.
            {
                shortRecord.Add(currShortObject);
                flag = false;
            }
        }

        int shortEstimate = LongObjects[currentQuestionNo].GetComponent<LongObjectClass>().ShortEstimate[currShortObject];
        correctEasyAns = shortEstimate;
        shortEasyObjName = ShortObjects[currShortObject].name;

        // Creates grid columns based on the number of the short objects
        ShortContainer.GetComponent<LetCGridLayout>().col = shortEstimate;
        // Creates cell prefab containing the short object GameObject -- cell prefabs is inserted in the columns
        ShortContainer.GetComponent<LetCGridLayout>().cellPrefab = ShortObjects[currShortObject];

        // Average Level
        if (DIFFICULTY == "Average") {
            int noMissing = Random.Range(1, shortEstimate-2);
            ShortContainer.GetComponent<LetCGridLayout>().noMissing = noMissing; 
            Color c = new Color32(0, 0, 0, 255); 
            // Creates cell prefab containing the short object GameObject -- cell prefabs is inserted in the columns
            missingShortObj = GameObject.Instantiate(ShortObjects[currShortObject]);
            missingShortObj.GetComponent<Image>().color = c;
            ShortContainer.GetComponent<LetCGridLayout>().cellPrefab2 = missingShortObj;
            correctEasyAns = noMissing;
        }

        // Setup the grid cells
        ShortContainer.GetComponent<LetCGridLayout>().SetCells();
    }

}
