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
    public GameObject[] hardSmallCapacityObjects;

    public GameObject[] AVE_CapacitySlots;

    [Header("Level Change")]
    public string CATEGORY;
    public string DIFFICULTY;

    [Header("Level Container")]
    public GameObject EasyQContainer;
    public GameObject AverageQContainer;
    public GameObject HardQContainer;

    [Header("Quiz Options")]
    public GameObject[] Options;
    public GameObject SubmitButton;
    public GameObject[] HardOptions;

    [Header("Game Object Container")]

    public GameObject[] EasyContainer;
    public GameObject[] AverContainer;
    public GameObject[] HardContainer;

    public GameObject[] txtContainer;

    public QuizTopUI quizTopUI;
    List<int> arrRecord = new List<int>();
    private List<int> smallRecord = new List<int>();

    private string firstObjName;
    private string secondObjName;
    private int left;
    private int right;
    private int correctAns;
    private string smallHardObjName;

    public int currentQuestionNo;
    public float timeLimit;
    private int score;
    private float timer;
    private bool stopTimer;

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
            EasyQContainer.SetActive(true);
            timeLimit = 90;
        }

        if (DIFFICULTY == "Average")
        {
            quizTopUI.Difiiculty.text = "Average";
            ResultPanel.GetComponent<QuizResultAnim>().setQuiz("Capacity", "Average");
            AverageQContainer.SetActive(true);
            timeLimit = 60;
        }

        if (DIFFICULTY == "Hard")
        {
            quizTopUI.Difiiculty.text = "Hard";
            ResultPanel.GetComponent<QuizResultAnim>().setQuiz("Capacity", "Hard");
            HardQContainer.SetActive(true);
            timeLimit = 30;
        }

        // Timer
        stopTimer = false;
        quizTopUI.TimerSlider.maxValue = timeLimit;
        quizTopUI.TimerSlider.value = timeLimit;

        DurstenfeldShuffle(CapacityObjects);
        DurstenfeldShuffle(aveCapacityObjects);
        DurstenfeldShuffle(hardCapacityObjects);
        DurstenfeldShuffle(hardSmallCapacityObjects);
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
                // int question = UnityEngine.Random.Range(0, QuestionList.Length);
                // quizTopUI.Question.text = QuestionList[question];
                //quizTopUI.Question.text = "     HOLDS __ __ __ __ THAN    ";

                EasyQuestion();
                SetAnswers();
            }

            if (DIFFICULTY == "Average")
            {
                quizTopUI.Difiiculty.text = "Average";
                quizTopUI.Question.text = "Arrange the objects from LEAST to GREATEST capacity";
                AverageQuestion();
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
            string[] choice = {"MORE", "LESS"};
            DurstenfeldShuffle(Options);

            for (int i = 0; i < Options.Length; i++)
            {
                Options[i].GetComponent<CapacityAnswerScript>().isCorrect = false;

                Options[0].transform.GetChild(0).GetComponent<TMP_Text>().text = choice[0];
                Options[1].transform.GetChild(0).GetComponent<TMP_Text>().text = choice[1];
            }

            if (left < right)
            {
                Options[1].GetComponent<CapacityAnswerScript>().isCorrect = true;
            }
            else if (left > right)
            {
                Options[0].GetComponent<CapacityAnswerScript>().isCorrect = true;
            }
        }

        if (DIFFICULTY == "Average")
        {
            SubmitButton.transform.GetChild(0).GetComponent<TMP_Text>().text = "Submit";
        }

        if (DIFFICULTY == "Hard")
        {
            int[] hardAnswers = { correctAns + 3, correctAns };
            DurstenfeldShuffle(hardAnswers);

            for (int i = 0; i < HardOptions.Length; i++)
            {
                if (HardOptions[i] != null) // add null check
                {
                    HardOptions[i].GetComponent<CapacityAnswerScript>().isCorrect = false;
                    HardOptions[i].transform.GetChild(0).GetComponent<TMP_Text>().text = hardAnswers[i].ToString() + " " + smallHardObjName;

                    // Condition for multiple choice type of question
                    if (hardAnswers[i] == correctAns) // checks if answer and button matches > matched button is set to True
                    {
                        HardOptions[i].GetComponent<CapacityAnswerScript>().isCorrect = true;
                    }
                }
            }
        }

    }

    public void AverageSubmit()
    {
        int first_iden = 0;
        int second_iden = 0;
        int third_iden = 0;

        //List<int> AveArrayCapacity = new List<int>();

        for (int i = 0; i < AverContainer.Length; i++)
        {

            if (Mathf.Round(AverContainer[i].transform.GetChild(0).position.x) == Mathf.Round(AVE_CapacitySlots[0].transform.position.x))
            {
                first_iden = AverContainer[i].transform.GetChild(0).GetComponent<ItemCapacity>().capacity;
            }
            else if (Mathf.Round(AverContainer[i].transform.GetChild(0).position.x) == Mathf.Round(AVE_CapacitySlots[1].transform.position.x))
            {
                second_iden = AverContainer[i].transform.GetChild(0).GetComponent<ItemCapacity>().capacity;
            }
            else if (Mathf.Round(AverContainer[i].transform.GetChild(0).position.x) == Mathf.Round(AVE_CapacitySlots[2].transform.position.x))
            {
                third_iden = AverContainer[i].transform.GetChild(0).GetComponent<ItemCapacity>().capacity;
            }
        }

        if (first_iden < second_iden && third_iden > second_iden)
        {
            SubmitButton.GetComponent<CapacityAnswerScript>().isCorrect = true;
            Debug.Log(" - first: " + first_iden);
            Debug.Log(" - second: " + second_iden);
            Debug.Log(" - third: " + third_iden);

        }
        else
        {
            SubmitButton.GetComponent<CapacityAnswerScript>().isCorrect = false;
        }

    }

    private void EasyQuestion()
    {
        GameObject firstContainer = EasyContainer[0];
        GameObject secondContainer = EasyContainer[1];

        if (EasyContainer[0].transform.childCount > 0)
        {
            Object.Destroy(EasyContainer[0].transform.GetChild(0).gameObject);
            Object.Destroy(EasyContainer[1].transform.GetChild(0).gameObject);
        }

        int currFirstObject = Random.Range(0, CapacityObjects.Length);
        int currSecondObject = Random.Range(0, CapacityObjects.Length);

        bool flag = true;
        while (flag)
        {
            currFirstObject = Random.Range(0, CapacityObjects.Length);
            currSecondObject = Random.Range(0, CapacityObjects.Length);
            if (arrRecord.Contains(currFirstObject) == false) // checks if the object was already used in question.
            {
                arrRecord.Add(currFirstObject);
                flag = false;
            }
            
            if (arrRecord.Contains(currSecondObject) == false) // checks if the object was already used in question.
            {
                arrRecord.Add(currSecondObject);
                flag = false;
            }

            if (CapacityObjects[currFirstObject].transform.GetComponent<ItemCapacity>().capacity == CapacityObjects[currSecondObject].transform.GetComponent<ItemCapacity>().capacity)
            {
                arrRecord.Remove(currSecondObject);
                flag = true;
            }
            // else if (currFirstObject != currSecondObject && arrRecord.Contains(currFirstObject) && arrRecord.Contains(currSecondObject))
            // {
            //     arrRecord.Remove(currSecondObject);
            //     flag = true;
            // }
        }

        firstObjName = CapacityObjects[currFirstObject].name;
        left = CapacityObjects[currFirstObject].GetComponent<ItemCapacity>().capacity;
        secondObjName = CapacityObjects[currSecondObject].name;
        right = CapacityObjects[currSecondObject].GetComponent<ItemCapacity>().capacity;

        GameObject leftObject = CapacityObjects[currFirstObject];
        GameObject rightObject = CapacityObjects[currSecondObject];

        Instantiate(leftObject, firstContainer.transform);
        Instantiate(rightObject, secondContainer.transform);

        quizTopUI.Question.text = "<color=#ffcb2b>" + firstObjName + "</color>  HOLDS __ __ __ __ THAN THE  <color=#ffcb2b>" + secondObjName + "</color>";
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

        // float parent_width = AverContainer[0].GetComponent<RectTransform>().rect.width;
        // float parent_height = AverContainer[0].GetComponent<RectTransform>().rect.height;

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
            //aveCapacityObjects[i].GetComponent<RectTransform>().sizeDelta = new Vector2(parent_width, parent_height);
        }
    }

    private void HardQuestion()
    {
        // Game Objects Instances
        GameObject ifBig = HardContainer[0];
        GameObject equalSmall = HardContainer[1];
        GameObject thenBig = HardContainer[2];

        int shortCapacity = 0;
        int multiplierCapacity = 0;

        if (HardContainer[0].transform.childCount > 0)
        {
            Object.Destroy(HardContainer[0].transform.GetChild(0).gameObject);
            Object.Destroy(HardContainer[1].transform.GetChild(0).gameObject);
            Object.Destroy(HardContainer[2].transform.GetChild(0).gameObject);
        }

        int currParentObject = Random.Range(0, hardCapacityObjects.Length);
        int currEqualObject = Random.Range(0, hardSmallCapacityObjects.Length);

        bool flag = true;
        while (flag)
        {
            currParentObject = Random.Range(0, hardCapacityObjects.Length);
            if (arrRecord.Contains(currParentObject) == false) // checks if the object was already used in question.
            {
                arrRecord.Add(currParentObject);
                flag = false;
            }
            currEqualObject = Random.Range(0, hardSmallCapacityObjects.Length);
            if (smallRecord.Contains(currEqualObject) == false) // checks if the object was already used in question.
            {
                smallRecord.Add(currEqualObject);
                flag = false;
            }
        }
        int[] smallEstimate = hardCapacityObjects[currParentObject].GetComponent<BigObjectClass>().smallEstimate;
        DurstenfeldShuffle(smallEstimate);
        int[] multiplier = hardCapacityObjects[currParentObject].GetComponent<MultiplierClassScript>().multiplier;
        DurstenfeldShuffle(multiplier);


        for (int i = 0; i < smallEstimate.Length; i++)
        {
            shortCapacity = hardCapacityObjects[currParentObject].GetComponent<BigObjectClass>().smallEstimate[i];
        }

        for (int i = 0; i < multiplier.Length; i++)
        {
            multiplierCapacity = hardCapacityObjects[currParentObject].GetComponent<MultiplierClassScript>().multiplier[i];
        }

        correctAns = shortCapacity * multiplierCapacity;
        smallHardObjName = hardSmallCapacityObjects[currEqualObject].name;

        for (int i = 0; i < txtContainer.Length; i++)
        {
            txtContainer[0].transform.GetComponent<TMP_Text>().text = shortCapacity.ToString();
            txtContainer[1].transform.GetComponent<TMP_Text>().text = multiplierCapacity.ToString();
        }

        GameObject smallObject = hardSmallCapacityObjects[currEqualObject];
        GameObject compareObject = hardCapacityObjects[currParentObject];

        Instantiate(compareObject, ifBig.transform);
        Debug.Log("Instantiated compareObject in ifBig");

        Instantiate(smallObject, equalSmall.transform);
        Debug.Log("Instantiated smallObject in equalSmall");

        Instantiate(compareObject, thenBig.transform);
        Debug.Log("Instantiated compareObject in thenBig");
    }
}
