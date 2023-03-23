using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("Hard Grid")]
    public GameObject QuestionObject;
    public GameObject QuestionAGrid;
    public GameObject QuestionBGrid;

    public QuizTopUI quizTopUI;
    List<int> arrRecord = new List<int>();
    private List<int> smallRecord = new List<int>();

    private string firstObjName;
    private string secondObjName;
    private int left;
    private int right;
    private int correctAns;
    private string smallHardObjName;
    private int currHardSmall;
    private int currHardObjNo;
    private int multiplier;

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
    public AudioSource correctSFX;
    public AudioSource wrongSFX;

    void Start()
    {
        quizTopUI.Category.text = QuizData.CATEGORY;
        DIFFICULTY = QuizData.DIFFICULTY;

        if (DIFFICULTY == "Easy")
        {
            quizTopUI.Difiiculty.text = "Easy";
            ResultPanel.GetComponent<QuizResultAnim>().setQuiz("Capacity", "Easy");
            EasyQContainer.SetActive(true);
            timeLimit = 30;
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
            timeLimit = 120;
        }

        // Timer
        stopTimer = false;
        quizTopUI.TimerSlider.maxValue = timeLimit;
        quizTopUI.TimerSlider.value = timeLimit;

        DurstenfeldShuffle(CapacityObjects);
        DurstenfeldShuffle(aveCapacityObjects);
        DurstenfeldShuffle(hardCapacityObjects);
        GenerateQuestion();

    }

    // Shuffle Algorithm
    private void DurstenfeldShuffle<T>(T[] gameObjectArr)
    {
        int last_index = gameObjectArr.Length - 1;
        while (last_index > 0)
        {
            int rand_index = UnityEngine.Random.Range(0, last_index + 1);
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
                EasyQuestion();
                SetAnswers();
            }

            if (DIFFICULTY == "Average")
            {
                quizTopUI.Difiiculty.text = "Average";
                quizTopUI.Question.text = "Arrange the objects from <color=#ffcb2b>least</color> to <color=#ffcb2b>greatest</color> capacity";
                AverageQuestion();
            }

            if (DIFFICULTY == "Hard")
            {
                quizTopUI.Difiiculty.text = "Hard";
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

        quizTopUI.Score.text = (score).ToString() + " / " + CapacityObjects.Length.ToString();
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
            int[] hardAnswers = new int[2];
            string sign = (UnityEngine.Random.value > 0.5f) ? "-" : "+"; // Randomly choose between "-" and "+"
            // Assign values to the hard answers array
            if (sign == "-" && ((multiplier - 1) != 0))
            {
                hardAnswers[0] = multiplier - 1;
                hardAnswers[1] = multiplier;
            }
            else
            {
                hardAnswers[0] = multiplier + 1;
                hardAnswers[1] = multiplier;
            }
            DurstenfeldShuffle(hardAnswers);

            for (int i = 0; i < HardOptions.Length; i++)
            {
                if (HardOptions[i] != null) // add null check
                {
                    HardOptions[i].GetComponent<CapacityAnswerScript>().isCorrect = false;
                    GameObject OptionGrid = HardOptions[i].transform.GetChild(0).gameObject;
                    // Destroy child objects in grid
                    if (OptionGrid.transform.childCount > 0)
                    {
                        for (var j = OptionGrid.transform.childCount - 1; j >= 0; j--)
                        {
                            Object.Destroy(OptionGrid.transform.GetChild(j).gameObject);
                        }
                    }
                    // setup grid
                    OptionGrid.GetComponent<CapacityGridLayout>().row = currHardObjNo;
                    switch (currHardObjNo) 
                    {
                            case 2:
                                OptionGrid.GetComponent<CapacityGridLayout>().columns = new int[] {hardAnswers[i], hardAnswers[i]};
                                break;
                            case 3:
                                OptionGrid.GetComponent<CapacityGridLayout>().columns = new int[] {hardAnswers[i], hardAnswers[i], hardAnswers[i]};
                                break;
                            case 4:
                                OptionGrid.GetComponent<CapacityGridLayout>().columns = new int[] {hardAnswers[i], hardAnswers[i], hardAnswers[i], hardAnswers[i]};
                                break;
                            default:
                                OptionGrid.GetComponent<CapacityGridLayout>().columns = new int[] {hardAnswers[i], hardAnswers[i], hardAnswers[i], hardAnswers[i], hardAnswers[i]};
                                break;
                    }
                    OptionGrid.GetComponent<CapacityGridLayout>().cellPrefab = hardSmallCapacityObjects[currHardSmall];
                    OptionGrid.GetComponent<CapacityGridLayout>().SetCells();

                    // multiplier * currHardObjNo
                    if (hardAnswers[i] * currHardObjNo == correctAns)
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
        }

        firstObjName = CapacityObjects[currFirstObject].GetComponent<LabelScriptClass>().objLabel;
        left = CapacityObjects[currFirstObject].GetComponent<ItemCapacity>().capacity;
        secondObjName = CapacityObjects[currSecondObject].GetComponent<LabelScriptClass>().objLabel;
        right = CapacityObjects[currSecondObject].GetComponent<ItemCapacity>().capacity;

        GameObject leftObject = CapacityObjects[currFirstObject];
        GameObject rightObject = CapacityObjects[currSecondObject];

        Instantiate(leftObject, firstContainer.transform);
        Instantiate(rightObject, secondContainer.transform);

        quizTopUI.Question.text = "<color=#ffcb2b>" + firstObjName + "</color> holds __ __ __ __ than the <color=#ffcb2b>" + secondObjName + "</color>";
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
        }
    }

    private void HardQuestion() {
        if (QuestionObject.transform.childCount > 0)
        {
            Object.Destroy(QuestionObject.transform.GetChild(0).gameObject);
        }
        if (QuestionAGrid.transform.childCount > 0)
        {
            for (var i = QuestionAGrid.transform.childCount - 1; i >= 0; i--)
            {
                Object.Destroy(QuestionAGrid.transform.GetChild(i).gameObject);
            }
        }
        if (QuestionBGrid.transform.childCount > 0)
        {
            for (var i = QuestionBGrid.transform.childCount - 1; i >= 0; i--)
            {
                Object.Destroy(QuestionBGrid.transform.GetChild(i).gameObject);
            }
        }

        GameObject MainObject;
        MainObject = GameObject.Instantiate(hardCapacityObjects[currentQuestionNo], QuestionObject.transform);

        RectTransform _rTransform = MainObject.GetComponent<RectTransform>();
        float _rWidth = QuestionObject.GetComponent<RectTransform>().rect.width;
        float _rHeight = QuestionObject.GetComponent<RectTransform>().rect.height;
        _rTransform.sizeDelta = new Vector2(_rWidth, _rHeight);
        
        currHardSmall = Random.Range(0, 6); // select random small object
        multiplier = hardCapacityObjects[currentQuestionNo].GetComponent<MultiplierClassScript>().multiplier[currHardSmall];

        if (multiplier > 5) {
            int tmp = Mathf.FloorToInt(multiplier/2);
            QuestionAGrid.GetComponent<CapacityGridLayout>().row = 2;
            QuestionAGrid.GetComponent<CapacityGridLayout>().columns = new int[] {multiplier-tmp, tmp};
        } else {
            QuestionAGrid.GetComponent<CapacityGridLayout>().row = 1;
            QuestionAGrid.GetComponent<CapacityGridLayout>().columns = new int[] {multiplier};
        }
        QuestionAGrid.GetComponent<CapacityGridLayout>().cellPrefab = hardSmallCapacityObjects[currHardSmall];
        QuestionAGrid.GetComponent<CapacityGridLayout>().SetCells();


        currHardObjNo = Random.Range(2, 4);
        QuestionBGrid.GetComponent<CapacityGridLayout>().columns = new int[] {currHardObjNo};
        QuestionBGrid.GetComponent<CapacityGridLayout>().cellPrefab = hardCapacityObjects[currentQuestionNo];
        QuestionBGrid.GetComponent<CapacityGridLayout>().SetCells();

        smallHardObjName = hardSmallCapacityObjects[currHardSmall].GetComponent<LabelScriptClass>().objLabel;
        correctAns = multiplier * currHardObjNo;

        firstObjName = hardCapacityObjects[currentQuestionNo].GetComponent<LabelScriptClass>().objLabel;
        secondObjName = hardSmallCapacityObjects[currHardSmall].GetComponent<LabelScriptClass>().objLabel;
        quizTopUI.Question.text = multiplier + " " + secondObjName + " are needed to fill 1 " + firstObjName + ". How many <color=#ffcb2b>" + secondObjName + "</color> are needed to fill <color=#ffcb2b>" + currHardObjNo + " " + firstObjName + "</color>." ;
    }
}

