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
    private int score;

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
        DurstenfeldShuffle(LongObjects);
        GenerateQuestion();
        audioSource = GetComponent<AudioSource>();
    }

    private void GenerateQuestion() 
    {
        if (currentQuestionNo < 10)
        {
            quizTopUI.QuestionNo.text = "Question " + (currentQuestionNo+1).ToString();
            EasyQuestion();
            SetAnswers();
            currentQuestionNo += 1;

        }
        else
        {
            Debug.Log("GAME OVER");
        }

    }

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

    public void SetAnswers()
    {
        int[] easyAnswersArr = { correctEasyAns, correctEasyAns+1, correctEasyAns-1};
        
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

    // public void Answer()
    // {
    //     if (currentQuestionNo < 9)
    //     {
    //         currentQuestionNo += 1;
    //         GenerateQuestion();

    //     }
    //     else
    //     {
    //         Debug.Log("GAME OVER");
    //     }
    // }

    IEnumerator nextQuestion(GameObject guiParentCanvas, float secondsToWait, string answer)
    {
        yield return new WaitForSeconds(secondsToWait);
        guiParentCanvas.GetComponent<OverlayPanel>().CloseOverlay();

        GenerateQuestion();

    }

    private void DurstenfeldShuffle(GameObject[] gameObjectArr) 
    {
        int last_index = gameObjectArr.Length - 1;
        while (last_index > 0)
        {
            int rand_index = Random.Range(0, last_index);
            GameObject temp = gameObjectArr[last_index];
            gameObjectArr[last_index] = gameObjectArr[rand_index];
            gameObjectArr[rand_index] = temp;
            last_index -= 1;
        }
    }

    public void correct()
    {
        score += 1;
        quizTopUI.Score.text = (score).ToString() + " / " + LongObjects.Length.ToString();
        CorrectOverlay.SetActive(true);
        audioSource.PlayOneShot(correctSFX);
        StartCoroutine(nextQuestion(CorrectOverlay, 2.0f, "correct"));
    }

    public void wrong()
    {
        WrongOverlay.SetActive(true);
        audioSource.PlayOneShot(wrongSFX);
        StartCoroutine(nextQuestion(WrongOverlay, 2.0f, "wrong"));
    }


}
