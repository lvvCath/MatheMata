using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using TMPro;

public class CapacityQuizManager : MonoBehaviour
{
    [Header("Game Objects")]
    public GameObject[] CapacityObjects;
    public GameObject[] aveCapacityObjects;
    public GameObject[] hardCapacityObjects;
    
    // AVERAGE Capacity Answer
    public GameObject[] AVE_CapacitySlots;
    public GameObject[] AVE_CapacityAnswer;

    [Header("Level Change")]
    public string CATEGORY;
    public string DIFFICULTY;

    [Header("Quiz Options")]
    public GameObject[] Options;

    [Header("Game Object Container")]

    public GameObject[] EasyContainer;
    public GameObject[] AverContainer;
    public GameObject[] HardContainer;
    public QuizTopUI quizTopUI;
    List<int> arrRecord = new List<int>();

    public int currentQuestionNo;
    public float timeLimit;
    private int score;
    private float timer;
    private bool stopTimer;
    private string[] QuestionList = { "Which of the following holds less?", "Which of the following holds more?" };

    [Header("PopUp Overlay Panel")]
    public GameObject WrongOverlay;
    public GameObject CorrectOverlay;

    public GameObject ResultPanel;

    [Header("Audio SFX")]
    public AudioClip correctSFX;
    public AudioClip wrongSFX;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        // Set Here the current Text for Question, Category, Difficulty. use condition

        quizTopUI.Category.text = QuizData.CATEGORY;

        DIFFICULTY = QuizData.DIFFICULTY;

        audioSource = GetComponent<AudioSource>();

        if (DIFFICULTY == "Easy")
        {
            quizTopUI.Difiiculty.text = "Easy";
            ResultPanel.GetComponent<QuizResultAnim>().setQuiz("Capacity", "Easy");
            //EasyContainer.SetActive(true);
            //timeLimit = 90;
        }

        if (DIFFICULTY == "Average")
        {
            quizTopUI.Difiiculty.text = "Average";
            ResultPanel.GetComponent<QuizResultAnim>().setQuiz("Capacity", "Average");
            //AverContainer.SetActive(true);
            //timeLimit = 60;
        }

        if (DIFFICULTY == "Hard")
        {
            quizTopUI.Difiiculty.text = "Hard";
            ResultPanel.GetComponent<QuizResultAnim>().setQuiz("Capacity", "Hard");
        }

        // Timer
        stopTimer = false;
        quizTopUI.TimerSlider.maxValue = timeLimit;
        quizTopUI.TimerSlider.value = timeLimit;

        DurstenfeldShuffle(CapacityObjects);
        //DurstenfeldShuffle(aveCapacityObjects);
        GenerateQuestion();

    }



    // Shuffle Algorithm
    private void DurstenfeldShuffle<T>(T[] gameObjectArr)
    {
        int last_index = gameObjectArr.Length - 1;
        while (last_index > 0)
        {
            int rand_index = UnityEngine.Random.Range(0, last_index + 1); //modify in documentation "+1"
            T temp = gameObjectArr[last_index];
            gameObjectArr[last_index] = gameObjectArr[rand_index];
            gameObjectArr[rand_index] = temp;
            last_index -= 1;
        }
    }

    private void Update()
    {
        if (stopTimer == false)
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
        if (currentQuestionNo < 10)
        {
            // Timer
            stopTimer = false;
            timer = timeLimit;

            quizTopUI.QuestionNo.text = "Question " + (currentQuestionNo + 1).ToString();

            if (DIFFICULTY == "Easy")
            {
                quizTopUI.Difiiculty.text = "Easy";
                int question = UnityEngine.Random.Range(0, QuestionList.Length);
                quizTopUI.Question.text = QuestionList[question];
                //quizTopUI.Question.text = "     HOLDS __ __ __ __ THAN    ";

                EasyQuestion();
                SetAnswers();
            }

            if (DIFFICULTY == "Average")
            {
                quizTopUI.Difiiculty.text = "Average";
                // int question = UnityEngine.Random.Range(0, QuestionList2.Length);
                // quizTopUI.Question.text = QuestionList2[question];
                quizTopUI.Question.text = "Arrange the objects from LEAST to GREATEST capacity";
                AverageQuestion();
                //SetAnswers();
            }

            if (DIFFICULTY == "Hard")
            {
                quizTopUI.Difiiculty.text = "Hard";
                quizTopUI.Question.text = "Select the correct capacity of the object";
                HardQuestion();
                SetAnswers();
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
            ResultPanel.GetComponent<QuizResultAnim>().setScore(score.ToString(), CapacityObjects.Length.ToString());

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
        Debug.Log("Correct");
        score += 1;
        stopTimer = true; // Timer

        quizTopUI.Score.text = (score).ToString() + " / " + CapacityObjects.Length.ToString();
        CorrectOverlay.SetActive(true);
        audioSource.PlayOneShot(correctSFX);
        StartCoroutine(nextQuestion(CorrectOverlay, 2.0f, "correct"));
    }

    public void wrong()
    {
        Debug.Log("Wrong");
        stopTimer = true; // Timer

        WrongOverlay.SetActive(true);
        audioSource.PlayOneShot(wrongSFX);
        StartCoroutine(nextQuestion(WrongOverlay, 2.0f, "wrong"));
    }

    public void SetAnswers()
    {

        if (DIFFICULTY == "Easy")
        {
            List<int> arrayCapacity = new List<int>();
            int ind;
            // string choice1 = "MORE";
            // string choice2 = "LESS";

            for (int i = 0; i < Options.Length; i++)
            {
                Options[i].GetComponent<CapacityAnswerScript>().isCorrect = false;
                //Options[0].transform.GetChild(0).GetComponent<TMP_Text>().text = choice1;
                //Options[1].transform.GetChild(0).GetComponent<TMP_Text>().text = choice2;
                Options[i].transform.GetChild(0).GetComponent<TMP_Text>().text = CapacityObjects[arrRecord[i]].name;
                arrayCapacity.Add(CapacityObjects[arrRecord[i]].GetComponent<ItemCapacity>().capacity);
            }
            int correctEasyAns;
            if (quizTopUI.Question.text == QuestionList[0])
            {
                correctEasyAns = arrayCapacity.Min();
                ind = arrayCapacity.IndexOf(correctEasyAns);
                Options[ind].GetComponent<CapacityAnswerScript>().isCorrect = true;

            }

            if (quizTopUI.Question.text == QuestionList[1])
            {
                correctEasyAns = arrayCapacity.Max();
                ind = arrayCapacity.IndexOf(correctEasyAns);
                Options[ind].GetComponent<CapacityAnswerScript>().isCorrect = true;

            }
        }

        if (DIFFICULTY == "Average")
        {

        }

        if (DIFFICULTY == "Hard")
        {

        }
    }

    private void EasyQuestion()
    {
        // Instantiate
        if (EasyContainer[0].transform.childCount > 0)
        {
            Object.Destroy(EasyContainer[0].transform.GetChild(0).gameObject);
            Object.Destroy(EasyContainer[1].transform.GetChild(0).gameObject);
        }

        float parent_width = EasyContainer[0].GetComponent<RectTransform>().rect.width;
        float parent_height = EasyContainer[0].GetComponent<RectTransform>().rect.height;
        arrRecord.Clear();
        for (int i = 0; i < 2; i++)
        {
            int currObject = Random.Range(0, CapacityObjects.Length);
            bool flag = true;
            while (flag)
            {
                currObject = Random.Range(0, CapacityObjects.Length);
                if (arrRecord.Contains(currObject) == false) // checks if the object was already used in question.
                {
                    arrRecord.Add(currObject);
                    flag = false;
                }
            }
            Instantiate(CapacityObjects[currObject], EasyContainer[i].transform);
            CapacityObjects[i].GetComponent<RectTransform>().sizeDelta = new Vector2(parent_width, parent_height);
        }
    }

    private void AverageQuestion()
    {
        // Instantiate
        if (AverContainer[0].transform.childCount > 0)
        {
            Object.Destroy(AverContainer[0].transform.GetChild(0).gameObject);
            Object.Destroy(AverContainer[1].transform.GetChild(0).gameObject);
            Object.Destroy(AverContainer[2].transform.GetChild(0).gameObject);
        }

        float parent_width = AverContainer[0].GetComponent<RectTransform>().rect.width;
        float parent_height = AverContainer[0].GetComponent<RectTransform>().rect.height;

        arrRecord.Clear();
        
        for (int i = 0; i < 3; i++)
        {
            int currObject = Random.Range(0, aveCapacityObjects.Length);
            bool flag = true;
            while (flag)
            {
                currObject = Random.Range(0, aveCapacityObjects.Length);
                if (arrRecord.Contains(currObject) == false) // checks if the object was already used in question.
                {
                    arrRecord.Add(currObject);
                    flag = false;
                }
            }
            Instantiate(aveCapacityObjects[currObject], AverContainer[i].transform);
            aveCapacityObjects[i].GetComponent<RectTransform>().sizeDelta = new Vector2(parent_width, parent_height);
        }
    }

    private void HardQuestion()
    {
        // to add function
    }
}
