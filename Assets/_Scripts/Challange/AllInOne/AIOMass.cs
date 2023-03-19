using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
public class AIOMass : MonoBehaviour
{
    [Header("Canvas")]
    public Canvas canvas;

    [Header("Game Objects")]
    public GameObject[] WeightedObjects;

    [Header("Average Game Objects")]
    public GameObject[] AverageObjects;
    public GameObject[] ScaleArr;

    [Header("Hard Game Objects")]
    public GameObject[] HeavyObjects;
    public GameObject[] LightObjects;
    public GameObject scaleBar;
    public GameObject LeftScale;
    public GameObject RightScale;
    
    [Header("Quiz Options")]
    public GameObject[] Options;
    public GameObject SubmitButton;

    [Header("Game Object Container")]
    public GameObject[] Container;
    public GameObject[] AverageContainer;
    public GameObject[] HardContainer;
    public QuizTopUI quizTopUI;
    List<int> arrRecord = new List<int>();
    List<int> arrLight = new List<int>();

    [Header("Quiz Values")]
    public int currentQuestionNo;
    private string[] QuestionList = {"Which of the following is the lightest?", "Which of the following is the heaviest?"};

    public void callStart()
    {
        DurstenfeldShuffle(WeightedObjects);
        DurstenfeldShuffle(HeavyObjects);
    }

    // Shuffle Algorithm
    private void DurstenfeldShuffle<T>(T[] gameObjectArr) 
    {
        int last_index = gameObjectArr.Length - 1;
        while (last_index > 0)
        {
            int rand_index = UnityEngine.Random.Range(0, last_index+1);
            T temp = gameObjectArr[last_index];
            gameObjectArr[last_index] = gameObjectArr[rand_index];
            gameObjectArr[rand_index] = temp;
            last_index -= 1;
        }
    }

    public void SetAnswers(string DIFFICULTY)
    {
        if (DIFFICULTY == "Easy"){
            List<int> arrayWeight = new List<int>();
            int ind;
            for (int i = 0; i < Options.Length; i++){
                Options[i].GetComponent<AIOAnswerScript>().isCorrect = false;
                Options[i].transform.GetChild(0).GetComponent<TMP_Text>().text = WeightedObjects[arrRecord[i]].name;
                arrayWeight.Add(WeightedObjects[arrRecord[i]].GetComponent<ItemWeight>().weight);
            }
            int correctEasyAns;
            if (quizTopUI.Question.text == QuestionList[0])
            {
                correctEasyAns = arrayWeight.Min();
                ind = arrayWeight.IndexOf(correctEasyAns);
                Options[ind].GetComponent<AIOAnswerScript>().isCorrect = true;
                    
            }

            if (quizTopUI.Question.text == QuestionList[1])
            {
                correctEasyAns = arrayWeight.Max();
                ind = arrayWeight.IndexOf(correctEasyAns);
                Options[ind].GetComponent<AIOAnswerScript>().isCorrect = true;
                    
            }
        }

        if (DIFFICULTY == "Average"){
            string[] correctans = {"Scale A", "Scale B", "Scale C"};
            for (int i = 0; i < Options.Length; i++) {
                Options[i].GetComponent<AIOAnswerScript>().isCorrect = false;
                Options[i].transform.GetChild(0).GetComponent<TMP_Text>().text = correctans[i];

            }
            int firstWeight =  AverageObjects[arrLight[0]].GetComponent<ItemWeight>().weight;
            int secondWeight = AverageObjects[arrLight[1]].GetComponent<ItemWeight>().weight;
            if (firstWeight > secondWeight)
            {
                Options[0].GetComponent<AIOAnswerScript>().isCorrect = true;
            }
            if (firstWeight < secondWeight)
            {
                Options[2].GetComponent<AIOAnswerScript>().isCorrect = true;
            }
            if (firstWeight == secondWeight)
            {
                Options[1].GetComponent<AIOAnswerScript>().isCorrect = true;
            }
        }

        if (DIFFICULTY == "Hard"){
            SubmitButton.transform.GetChild(0).GetComponent<TMP_Text>().text = "Submit";
        }
    }

    public void HardMassSubmit()
    {
        SubmitButton.GetComponent<AIOAnswerScript>().isCorrect = RightScale.GetComponent<TriggerScale>().IsEqual();
    }

    public void EasyQuestion()
    {
        int question = UnityEngine.Random.Range(0, QuestionList.Length);
        quizTopUI.Question.text = QuestionList[question];

        // Instantiate
        if (Container[0].transform.childCount > 0){
            Object.Destroy(Container[0].transform.GetChild(0).gameObject);
            Object.Destroy(Container[1].transform.GetChild(0).gameObject);
            Object.Destroy(Container[2].transform.GetChild(0).gameObject);
        }
        
        float parent_width = Container[0].GetComponent<RectTransform>().rect.width;
        float parent_height = Container[0].GetComponent<RectTransform>().rect.height;
        arrRecord.Clear();
        for (int i = 0; i < Container.Length; i++)
        {
            int currObject = Random.Range(0, WeightedObjects.Length);
            bool flag = true;
            while (flag)
            {
                currObject = Random.Range(0, WeightedObjects.Length);
                if (arrRecord.Contains(currObject) == false) // checks if the object was already used in question.
                {
                    arrRecord.Add(currObject);
                    flag = false;
                }
            }
            Instantiate(WeightedObjects[currObject], Container[i].transform);
            WeightedObjects[i].GetComponent<RectTransform>().sizeDelta = new Vector2(parent_width, parent_height);
        }
    }

    public void AverageQuestion()
    {
        // Instantiate the two objects based on the Child Container (Object1 and Object2)
        
        GameObject container;
        int counter = 0;
        int currObject = Random.Range(0, AverageObjects.Length);
        for (int i = 0; i < AverageContainer.Length; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                container = AverageContainer[i].transform.GetChild(j).gameObject;
                if (container.transform.childCount > 0)
                    {
                        Object.Destroy(container.transform.GetChild(0).gameObject);
                    }
            }
        }

        arrLight.Clear();
        for (int i = 0; i < AverageContainer.Length; i++)
        {
            for (int j = 0; j < AverageContainer[i].transform.childCount-1; j++)
            {

                bool flag = true;
                if (counter >= 2) 
                {
                    flag = false;
                }
                while (flag)
                {
                    currObject = Random.Range(0, AverageObjects.Length);
                    if (arrRecord.Contains(currObject) == false) // checks if the object was already used in question.
                    {
                        arrRecord.Add(currObject);
                        arrLight.Add(currObject);
                        flag = false;
                    }
                }
                container = AverageContainer[i].transform.GetChild(j).gameObject;
                GameObject newObj = Instantiate(AverageObjects[arrLight[j]], container.transform);

                // Get the RectTransform component of the instantiated object
                RectTransform newObjRect = newObj.GetComponent<RectTransform>();

                // Set the anchored position to the bottom of the child container
                newObjRect.anchorMin = new Vector2(0.5f, 0f);
                newObjRect.anchorMax = new Vector2(0.5f, 0f);
                newObjRect.pivot = new Vector2(0.5f, 0f);
                newObjRect.anchoredPosition = new Vector2(0f, 0f);
        counter++;
            }
        }
        quizTopUI.Question.text = "Pick the scale where<color=#ffcb2b> " + AverageObjects[arrLight[0]].name + "</color> and <color=#ffcb2b>" + AverageObjects[arrLight[1]].name + "</color> is in the right balance";
        
    }

    public void HardQuestion()
    {
        //GameObject[] Instances;
        GameObject heavy = HardContainer[0];
        GameObject light = HardContainer[1];
        int heavyObject_mass;
        if (heavy.transform.childCount > 0){
            Object.Destroy(heavy.transform.GetChild(0).gameObject);
        }
        for (int i = light.transform.childCount-1; i >= 0; i--)
        {
            GameObject.Destroy(light.transform.GetChild(i).gameObject);
        }
        
        int currHeavy = Random.Range(0, HeavyObjects.Length);
        int currLight = Random.Range(0, LightObjects.Length);

        bool flag = true;
            while (flag)
            {
                currHeavy = Random.Range(0, HeavyObjects.Length);
                if (arrRecord.Contains(currHeavy) == false) // checks if the object was already used in question.
                {
                    arrRecord.Add(currHeavy);
                    flag = false;
                }
                currLight = Random.Range(0, LightObjects.Length);
                if (arrLight.Contains(currLight) == false) // checks if the object was already used in question.
                {
                    arrLight.Add(currLight);
                    flag = false;
                }
            }
        GameObject currObject = LightObjects[currLight];
        GameObject leftObject = HeavyObjects[currHeavy];

        heavyObject_mass = leftObject.GetComponent<HeavyObjectClass>().lightEstimates[currLight];
        RightScale.GetComponent<TriggerScale>().SetLeftScaleMass(heavyObject_mass);
           
        currObject.GetComponent<ObjectFunction>().canvas = canvas;
        currObject.GetComponent<ObjectFunction>().container = light;

        quizTopUI.Question.text = "Drag n' Drop the <color=#ffcb2b>"+ currObject.name +"</color> to match the weight of <color=#ffcb2b>"+ HeavyObjects[currHeavy].name +"</color> on the scale";

        Instantiate(currObject, light.transform);
        Instantiate(leftObject, heavy.transform);
    }

    public void DestroyOnClick () {

        if(HardContainer[1].transform.childCount > 1)
        {
            Object.Destroy(HardContainer[1].transform.GetChild(0).gameObject);
        }
    }
}