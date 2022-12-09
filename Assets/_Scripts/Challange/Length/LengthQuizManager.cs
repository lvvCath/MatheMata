using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LengthQuizManager : MonoBehaviour
{
    [Header("Game Objects")]
    public GameObject[] LongObjects;
    public GameObject[] ShortObjects;

    [Header("Question Text")]
    public TMP_Text QuestionNoTxt;

    [Header("Game Object Container")]
    public GameObject ParentContainer;
    public GameObject ShortContainer;
    public GameObject LongContainer;

    public int currentQuestionNo;

    private List<int> shortRecord = new List<int>();

    private void Start()
    {
        DurstenfeldShuffle(LongObjects);
        GenerateQuestion();
    }

    private void GenerateQuestion() 
    {
        QuestionNoTxt.text = "Question " + (currentQuestionNo+1).ToString();
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

        ShortContainer.GetComponent<LetCGridLayout>().col = shortEstimate;
        ShortContainer.GetComponent<LetCGridLayout>().cellPrefab = ShortObjects[currShortObject];

        ShortContainer.GetComponent<LetCGridLayout>().SetCells();

    }

    public void Answer()
    {
        if (currentQuestionNo < 9)
        {
            currentQuestionNo += 1;
            GenerateQuestion();

        }
        else
        {
            Debug.Log("GAME OVER");
        }
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


}
