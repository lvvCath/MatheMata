using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
public class AIOCapacity : MonoBehaviour
{
    [Header("Quiz Top Panel")]
    public QuizTopUI quizTopUI;

    [Header("Game Objects")]
    public GameObject[] CapacityObjects;
    public GameObject[] aveCapacityObjects;
    public GameObject[] hardCapacityObjects;
    public GameObject[] hardSmallCapacityObjects;

    public GameObject[] AVE_CapacitySlots;

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

    public void callStart()
    {
        DurstenfeldShuffle(CapacityObjects);
        DurstenfeldShuffle(aveCapacityObjects);
        DurstenfeldShuffle(hardCapacityObjects);
    }

    public void DurstenfeldShuffle<T>(T[] gameObjectArr) 
    {
        int last_index = gameObjectArr.Length - 1;
        while (last_index > 0)
        {
            int rand_index = Random.Range(0, last_index+1);
            T temp = gameObjectArr[last_index];
            gameObjectArr[last_index] = gameObjectArr[rand_index];
            gameObjectArr[rand_index] = temp;
            last_index -= 1;
        }
    }

    public void SetAnswers(string DIFFICULTY)
    {

        if (DIFFICULTY == "Easy")
        {
            string[] choice = {"MORE", "LESS"};
            DurstenfeldShuffle(Options);

            for (int i = 0; i < Options.Length; i++)
            {
                Options[i].GetComponent<AIOAnswerScript>().isCorrect = false;

                Options[0].transform.GetChild(0).GetComponent<TMP_Text>().text = choice[0];
                Options[1].transform.GetChild(0).GetComponent<TMP_Text>().text = choice[1];
            }

            if (left < right)
            {
                Options[1].GetComponent<AIOAnswerScript>().isCorrect = true;
            }
            else if (left > right)
            {
                Options[0].GetComponent<AIOAnswerScript>().isCorrect = true;
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
                    HardOptions[i].GetComponent<AIOAnswerScript>().isCorrect = false;
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
                        HardOptions[i].GetComponent<AIOAnswerScript>().isCorrect = true;
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
            SubmitButton.GetComponent<AIOAnswerScript>().isCorrect = true;

        }
        else
        {
            SubmitButton.GetComponent<AIOAnswerScript>().isCorrect = false;
        }

    }

    public void EasyQuestion()
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
            if (arrRecord.Contains(currFirstObject) == false)
            {
                arrRecord.Add(currFirstObject);
                flag = false;
            }
            
            if (arrRecord.Contains(currSecondObject) == false)
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

        quizTopUI.Question.text = "<color=#ffcb2b>" + firstObjName + "</color>  HOLDS __ __ __ __ THAN THE  <color=#ffcb2b>" + secondObjName + "</color>";
    }

    public void AverageQuestion()
    {
        quizTopUI.Question.text = "Arrange the objects from LEAST to GREATEST capacity";
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
                if (arrRecord.Contains(currObject) == false)
                {
                    arrRecord.Add(currObject);
                    flag = false;
                }
            }
            Instantiate(aveCapacityObjects[currObject], AverContainer[i].transform);
        }
    }

    public void HardQuestion() {
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
        currentQuestionNo += 1 ;
    }

}
