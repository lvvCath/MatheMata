using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class LengthQuizManager : MonoBehaviour
{
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
    private int correctEasyAns;
    private string shortEasyObjName;

    [Header("PopUp Overlay Panel")]
    public GameObject WrongOverlay;
    public GameObject CorrectOverlay;

    [Header("Audio SFX")]
    public AudioClip correctSFX;
    public AudioClip wrongSFX;

    private AudioSource audioSource;

     private void Start()
    {
        // Set Here the current Text for Question, Category, Difficulty. use condition

        // if Easy
        quizTopUI.Category.text = "Length";
        quizTopUI.Difiiculty.text = "Easy";
        quizTopUI.Question.text = "How many shorter objects required to equal the length of the longer object?"; 


        stopTimer = false;
        quizTopUI.TimerSlider.maxValue = timeLimit;
        quizTopUI.TimerSlider.value = timeLimit;

        DurstenfeldShuffle(LongObjects);
        GenerateQuestion();
        audioSource = GetComponent<AudioSource>();
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
            stopTimer = false;
            timer = timeLimit;

            quizTopUI.QuestionNo.text = "Question " + (currentQuestionNo+1).ToString();

            // insert here if else condition to generate question based on selected lvl of difficulty
            EasyQuestion();
            SetAnswers();

            currentQuestionNo += 1;

        }
        else
        {
            // TO DO game result panel
            stopTimer = true;
            quizTopUI.Timer.text = "00:00";
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
        stopTimer = true;

        quizTopUI.Score.text = (score).ToString() + " / " + LongObjects.Length.ToString();
        CorrectOverlay.SetActive(true);
        audioSource.PlayOneShot(correctSFX);
        StartCoroutine(nextQuestion(CorrectOverlay, 2.0f, "correct"));
    }

    public void wrong()
    {
        stopTimer = true;

        WrongOverlay.SetActive(true);
        audioSource.PlayOneShot(wrongSFX);
        StartCoroutine(nextQuestion(WrongOverlay, 2.0f, "wrong"));
    }

// Length Easy Difficulty
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
            if (shortRecord.Contains(currShortObject) == false)
            {
                shortRecord.Add(currShortObject);
                flag = false;
            }
        }

        int shortEstimate = LongObjects[currentQuestionNo].GetComponent<LongObjectClass>().ShortEstimate[currShortObject];
        correctEasyAns = shortEstimate;
        shortEasyObjName = ShortObjects[currShortObject].name;
        ShortContainer.GetComponent<LetCGridLayout>().col = shortEstimate;
        ShortContainer.GetComponent<LetCGridLayout>().cellPrefab = ShortObjects[currShortObject];

        ShortContainer.GetComponent<LetCGridLayout>().SetCells();
    }

}
