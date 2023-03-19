using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class LengthQuizManager : MonoBehaviour
{
    [Header("Quiz Info")]
    public string CATEGORY;
    public string DIFFICULTY;

    [Header("Game Objects")]
    public GameObject[] LongObjects;
    public GameObject[] ShortObjects;
    public GameObject[] ShortObjectsOption;

    [Header("Easy & Average Quiz Options")]
    public GameObject[] Options;

    [Header("Hard Quiz Options")]
    public GameObject[] HardOptions;

    [Header("Main Question Container")]
    public GameObject EasyAverageContainer;
    public GameObject HardContainer;

    [Header("Easy & Ave Game Object Container")]
    public GameObject ParentContainer;
    public GameObject ShortContainer;
    public GameObject LongContainer;

    [Header("Hard Game Object Container")]
    public GameObject HardShortObjContainer;

    [Header("Quiz Top Panel")]
    public QuizTopUI quizTopUI;

    [Header("Quiz Values")]
    public int currentQuestionNo;
    public float timeLimit;
    private int score;
    private float timer;
    private bool stopTimer;

    private List<int> shortRecord = new List<int>();

    // Easy
    private int correctAns;
    private string shortEasyObjName;

    [Header("PopUp Overlay Panel")]
    public GameObject WrongOverlay;
    public GameObject CorrectOverlay;

    public GameObject ResultPanel;

    [Header("Audio SFX")]
    public AudioSource correctSFX;
    public AudioSource wrongSFX;

     private void Start()
    {
        quizTopUI.Category.text = QuizData.CATEGORY;
        
        DIFFICULTY = QuizData.DIFFICULTY;

        if (DIFFICULTY == "Easy") {
            quizTopUI.Difiiculty.text = "Easy";
            ResultPanel.GetComponent<QuizResultAnim>().setQuiz("Length", "Easy"); 
            EasyAverageContainer.SetActive(true);
            timeLimit = 90;
        }

        if (DIFFICULTY == "Average") {
            quizTopUI.Difiiculty.text = "Average";
            ResultPanel.GetComponent<QuizResultAnim>().setQuiz("Length", "Average");
            EasyAverageContainer.SetActive(true);
            timeLimit = 60;
        }

        if (DIFFICULTY == "Hard") {
            quizTopUI.Difiiculty.text = "Hard";
            ResultPanel.GetComponent<QuizResultAnim>().setQuiz("Length", "Hard");
            DurstenfeldShuffle(ShortObjectsOption);
            HardContainer.SetActive(true);
            timeLimit = 30;
        }

        // Timer
        stopTimer = false;
        quizTopUI.TimerSlider.maxValue = timeLimit;
        quizTopUI.TimerSlider.value = timeLimit;
        
        DurstenfeldShuffle(LongObjects);
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

            if (DIFFICULTY == "Easy" || DIFFICULTY == "Average") 
            {
                EasyAverageQuestion();
                SetAnswers();
            }
            if (DIFFICULTY == "Hard")  
            {
                HardQuestion();
                SetHardAnswers();
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
        }

    }

    public void SetAnswers()
    {
        int[] easyAnswersArr = { correctAns+1, correctAns, correctAns-1 };
        DurstenfeldShuffle(easyAnswersArr);
        
        for (int i = 0; i < Options.Length; i++)
        {
            Options[i].GetComponent<LengthAnswerScript>().isCorrect = false;
            Options[i].transform.GetChild(0).GetComponent<TMP_Text>().text = easyAnswersArr[i].ToString() + " " + shortEasyObjName;

            // Condition for multiple choice type of question
            if (easyAnswersArr[i] == correctAns) // checks if answer and button matches > matched button is set to True
            {
                Options[i].GetComponent<LengthAnswerScript>().isCorrect = true;
            } 
        }
    }

    public void SetHardAnswers()
    {
        int rand_opt = Random.Range(0, 2);
        
        for (int i = 0; i < HardOptions.Length; i++)
        {
            GameObject OptionObjContainer = HardOptions[i].transform.GetChild(0).gameObject;
            if (currentQuestionNo > 0) {
                Object.Destroy(OptionObjContainer.transform.GetChild(0).gameObject);
            }

            GameObject GameObj;

            if (rand_opt == 0) 
            {
                GameObj = GameObject.Instantiate(LongObjects[currentQuestionNo]);
                rand_opt = 1;
            }
            else
            {
                GameObj = GameObject.Instantiate(ShortObjectsOption[currentQuestionNo]);
                rand_opt = 0;
            }

            GameObj.SetActive(true);
            Instantiate(GameObj, OptionObjContainer.transform);

            // Condition for multiple choice type of question
            if (GameObj.GetComponent<ObjectInfo>().ObjectName == LongObjects[currentQuestionNo].GetComponent<ObjectInfo>().ObjectName) // checks if answer and button matches > matched button is set to True
            {
                HardOptions[i].GetComponent<LengthAnswerScript>().isCorrect = true;
            } 
            else 
            {
                HardOptions[i].GetComponent<LengthAnswerScript>().isCorrect = false;
            }

            StartCoroutine(DieGameObject(GameObj));
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

// Length Easy and Average Difficulty
    private void EasyAverageQuestion()
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
        correctAns = shortEstimate;
        shortEasyObjName = ShortObjects[currShortObject].name;

        // Creates grid columns based on the number of the short objects
        ShortContainer.GetComponent<LetCGridLayout>().col = shortEstimate;
        // Creates cell prefab containing the short object GameObject -- cell prefabs is inserted in the columns
        ShortContainer.GetComponent<LetCGridLayout>().cellPrefab = ShortObjects[currShortObject];

        if (DIFFICULTY == "Easy") {
            quizTopUI.Question.text = "How many <color=#ffcb2b>" + shortEasyObjName + "</color> required to equal the length of the <color=#ffcb2b>" + LongObjects[currentQuestionNo].name + "</color>?"; 
        }
        // Average Level
        if (DIFFICULTY == "Average") {
            int noMissing = Random.Range(2, shortEstimate-1);
            ShortContainer.GetComponent<LetCGridLayout>().noMissing = noMissing; 
            Color c = new Color32(0, 0, 0, 255); 
            // Creates cell prefab containing the short object GameObject -- cell prefabs is inserted in the columns
            GameObject missingShortObj;
            missingShortObj = GameObject.Instantiate(ShortObjects[currShortObject]);
            missingShortObj.GetComponent<Image>().color = c;
            ShortContainer.GetComponent<LetCGridLayout>().cellPrefab2 = missingShortObj;
            correctAns = noMissing;

            quizTopUI.Question.text = "How many <color=#ffcb2b>missing</color> " + shortEasyObjName + " required to equal the length of the " + LongObjects[currentQuestionNo].name + "?"; 

            // Destroy initialized missingShortObj after assigneing a copy to LetCGridLayout
            StartCoroutine(DieGameObject(missingShortObj));
        }

        // Setup the grid cells
        ShortContainer.GetComponent<LetCGridLayout>().SetCells();
    }

    
    private void HardQuestion() 
    {
        if (HardShortObjContainer.transform.childCount > 0)
        {
            for (var i = HardShortObjContainer.transform.childCount - 1; i >= 0; i--)
            {
                Object.Destroy(HardShortObjContainer.transform.GetChild(i).gameObject);
            }
        }

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
        shortEasyObjName = ShortObjects[currShortObject].name;

        // Creates grid columns based on the number of the short objects
        HardShortObjContainer.GetComponent<LetCGridLayout>().col = shortEstimate;
        // Creates cell prefab containing the short object GameObject -- cell prefabs is inserted in the columns
        HardShortObjContainer.GetComponent<LetCGridLayout>().cellPrefab = ShortObjects[currShortObject];

        HardShortObjContainer.GetComponent<LetCGridLayout>().SetCells();

        if (DIFFICULTY == "Hard") {
            quizTopUI.Question.text = "Question"; 
            // Which of the following objects has the length of NO OBJECT NAME?
            quizTopUI.Question.text = "Which of the following objects has the length of <color=#ffcb2b>" + shortEstimate + " " + shortEasyObjName + "</color>?"; 
        }

    }

    IEnumerator DieGameObject(GameObject gameobject){
     yield return new WaitForSeconds(0.1f); //waits 3 seconds
     Object.Destroy(gameobject);
    }

}


