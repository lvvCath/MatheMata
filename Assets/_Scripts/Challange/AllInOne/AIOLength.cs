using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class AIOLength : MonoBehaviour
{
    [Header("Quiz Top Panel")]
    public QuizTopUI quizTopUI;

    [Header("Length Game Objects")]
    public GameObject[] L_LongObj;
    public GameObject[] L_ShortObj;
    public GameObject[] L_ShortObjOption;

    [Header("Length Options")]
    public GameObject[] L_EA_Options;
    public GameObject[] L_H_Options;

    [Header("Length GameObject Container")]
    public GameObject L_EA_ParentCont;
    public GameObject L_EA_ShortCont;
    public GameObject L_H_ShortObjCont;

    // private variables
    private int correctAns;

    // Easy private variables
    private List<int> L_shortRecord = new List<int>();
    private int L_currentQuestionNo = 0;
    private string L_shortObjName;

    private void Start()
    {
        DurstenfeldShuffle(L_LongObj);
        DurstenfeldShuffle(L_ShortObjOption);
    }

    public void DurstenfeldShuffle<T>(T[] gameObjectArr) 
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
    public void L_EA_SetAnswers()
    {
        int[] easyAnswersArr = { correctAns+1, correctAns, correctAns-1 };
        DurstenfeldShuffle(easyAnswersArr);
        
        for (int i = 0; i < L_EA_Options.Length; i++)
        {
            L_EA_Options[i].GetComponent<AIOAnswerScript>().isCorrect = false;
            L_EA_Options[i].transform.GetChild(0).GetComponent<TMP_Text>().text = easyAnswersArr[i].ToString() + " " + L_shortObjName;

            if (easyAnswersArr[i] == correctAns)
            {
                L_EA_Options[i].GetComponent<AIOAnswerScript>().isCorrect = true;
            } 
        }
    }

    public void L_H_SetAnswers(int currentQuestionNo)
    {
        int rand_opt = Random.Range(0, 2);
        
        for (int i = 0; i < L_H_Options.Length; i++)
        {
            GameObject OptionObjContainer = L_H_Options[i].transform.GetChild(0).gameObject;
            if (OptionObjContainer.transform.childCount > 0) {
                Object.Destroy(OptionObjContainer.transform.GetChild(0).gameObject);
            }

            GameObject GameObj;

            if (rand_opt == 0) 
            {
                GameObj = GameObject.Instantiate(L_LongObj[currentQuestionNo]);
                rand_opt = 1;
            }
            else
            {
                GameObj = GameObject.Instantiate(L_ShortObjOption[currentQuestionNo]);
                rand_opt = 0;
            }

            GameObj.SetActive(true);
            Instantiate(GameObj, OptionObjContainer.transform);

            // Condition for multiple choice type of question
            if (GameObj.GetComponent<ObjectInfo>().ObjectName == L_LongObj[currentQuestionNo].GetComponent<ObjectInfo>().ObjectName) // checks if answer and button matches > matched button is set to True
            {
                L_H_Options[i].GetComponent<AIOAnswerScript>().isCorrect = true;
            } 
            else 
            {
                L_H_Options[i].GetComponent<AIOAnswerScript>().isCorrect = false;
            }

            StartCoroutine(DieGameObject(GameObj));
        }
    }

    // Length Easy and Average Difficulty
    public void L_EA_Question(string DIFFICULTY)
    {
        if (L_EA_ShortCont.transform.childCount > 0)
        {
            for (var i = L_EA_ShortCont.transform.childCount - 1; i >= 0; i--)
            {
                Object.Destroy(L_EA_ShortCont.transform.GetChild(i).gameObject);
            }
        }
        if (L_currentQuestionNo != 0)
        {
            L_LongObj[L_currentQuestionNo-1].SetActive(false);
        }

        // Set Width of Parent Container using the long object width.
        float child_width = L_LongObj[L_currentQuestionNo].GetComponent<RectTransform>().rect.width;
        float parent_height = L_EA_ParentCont.GetComponent<RectTransform>().rect.height;

        L_EA_ParentCont.GetComponent<RectTransform>().sizeDelta = new Vector2(child_width, parent_height);

        L_LongObj[L_currentQuestionNo].SetActive(true);

        // Short Object
        int currShortObject = Random.Range(0, L_ShortObj.Length);
        bool flag = true;
        while (flag)
        {
            currShortObject = Random.Range(0, L_ShortObj.Length);
            if (L_shortRecord.Contains(currShortObject) == false) // checks if short object was already used in question.
            {
                L_shortRecord.Add(currShortObject);
                flag = false;
            }
        }

        int shortEstimate = L_LongObj[L_currentQuestionNo].GetComponent<LongObjectClass>().ShortEstimate[currShortObject];
        correctAns = shortEstimate;
        L_shortObjName = L_ShortObj[currShortObject].name;

        // Creates grid columns based on the number of the short objects
        L_EA_ShortCont.GetComponent<LetCGridLayout>().col = shortEstimate;
        // Creates cell prefab containing the short object GameObject -- cell prefabs is inserted in the columns
        L_EA_ShortCont.GetComponent<LetCGridLayout>().cellPrefab = L_ShortObj[currShortObject];

        if (DIFFICULTY == "Easy") {
            quizTopUI.Question.text = "How many <color=#ffcb2b>" + L_shortObjName + "</color> required to equal the length of the <color=#ffcb2b>" + L_LongObj[L_currentQuestionNo].name + "</color>?";
            L_EA_ShortCont.GetComponent<LetCGridLayout>().noMissing = 0;  
        }
        // Average Level
        if (DIFFICULTY == "Average") {
            int noMissing = Random.Range(2, shortEstimate-1);
            L_EA_ShortCont.GetComponent<LetCGridLayout>().noMissing = noMissing; 
            Color c = new Color32(0, 0, 0, 255); 
            // Creates cell prefab containing the short object GameObject -- cell prefabs is inserted in the columns
            GameObject missingShortObj;
            missingShortObj = GameObject.Instantiate(L_ShortObj[currShortObject]);
            missingShortObj.GetComponent<Image>().color = c;
            L_EA_ShortCont.GetComponent<LetCGridLayout>().cellPrefab2 = missingShortObj; 
            correctAns = noMissing;

            quizTopUI.Question.text = "How many <color=#ffcb2b>missing</color> " + L_shortObjName + " required to equal the length of the " + L_LongObj[L_currentQuestionNo].name + "?"; 

            // Destroy initialized missingShortObj after assigneing a copy to LetCGridLayout
            StartCoroutine(DieGameObject(missingShortObj));
        }

        // Setup the grid cells
        L_EA_ShortCont.GetComponent<LetCGridLayout>().SetCells();
        L_currentQuestionNo +=1;
    }

    
    public void L_H_Question(string DIFFICULTY, int currentQuestionNo) 
    {
        if (L_H_ShortObjCont.transform.childCount > 0)
        {
            for (var i = L_H_ShortObjCont.transform.childCount - 1; i >= 0; i--)
            {
                Object.Destroy(L_H_ShortObjCont.transform.GetChild(i).gameObject);
            }
        }

        // Short Object
        int currShortObject = Random.Range(0, L_ShortObj.Length);
        bool flag = true;
        while (flag)
        {
            currShortObject = Random.Range(0, L_ShortObj.Length);
            if (L_shortRecord.Contains(currShortObject) == false) // checks if short object was already used in question.
            {
                L_shortRecord.Add(currShortObject);
                flag = false;
            }
        }

        int shortEstimate = L_LongObj[currentQuestionNo].GetComponent<LongObjectClass>().ShortEstimate[currShortObject];
        L_shortObjName = L_ShortObj[currShortObject].name;

        // Creates grid columns based on the number of the short objects
        L_H_ShortObjCont.GetComponent<LetCGridLayout>().col = shortEstimate;
        // Creates cell prefab containing the short object GameObject -- cell prefabs is inserted in the columns
        L_H_ShortObjCont.GetComponent<LetCGridLayout>().cellPrefab = L_ShortObj[currShortObject];

        L_H_ShortObjCont.GetComponent<LetCGridLayout>().SetCells();

        if (DIFFICULTY == "Hard") {
            quizTopUI.Question.text = "Question"; 
            // Which of the following objects has the length of NO OBJECT NAME?
            quizTopUI.Question.text = "Which of the following objects has the length of <color=#ffcb2b>" + shortEstimate + " " + L_shortObjName + "</color>?"; 
        }

    }

    IEnumerator DieGameObject(GameObject gameobject){
     yield return new WaitForSeconds(0.1f); //waits 3 seconds
     Object.Destroy(gameobject);
    }


}