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
    public GameObject[] HardContainer;

    public GameObject[] txtContainer;

    List<int> arrRecord = new List<int>();
    private List<int> smallRecord = new List<int>();

    private string firstObjName;
    private string secondObjName;
    private int left;
    private int right;
    private int correctAns;
    private string smallHardObjName;

    public int currentQuestionNo;

    public void callStart()
    {
        DurstenfeldShuffle(CapacityObjects);
        DurstenfeldShuffle(aveCapacityObjects);
        DurstenfeldShuffle(hardCapacityObjects);
        DurstenfeldShuffle(hardSmallCapacityObjects);
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
            int[] hardAnswers = { correctAns + 3, correctAns };
            DurstenfeldShuffle(hardAnswers);

            for (int i = 0; i < HardOptions.Length; i++)
            {
                if (HardOptions[i] != null) // add null check
                {
                    HardOptions[i].GetComponent<AIOAnswerScript>().isCorrect = false;
                    HardOptions[i].transform.GetChild(0).GetComponent<TMP_Text>().text = hardAnswers[i].ToString() + " " + smallHardObjName;

                    if (hardAnswers[i] == correctAns)
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

    public void HardQuestion()
    {
        quizTopUI.Question.text = "Select the correct capacity of the object";
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
        Instantiate(smallObject, equalSmall.transform);
        Instantiate(compareObject, thenBig.transform);
    }
}
