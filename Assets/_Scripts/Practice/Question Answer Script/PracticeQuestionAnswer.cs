using UnityEngine;
[System.Serializable]
public class PracticeQuestionAnswer
{
    public string Question;
    public string[] Answers;
    public int CorrectAnswer;
    public string Category;
    public GameObject[] Background;
    public GameObject Hint;

    // AVERAGE Capacity Answer
    public GameObject[] AVE_CapacitySlots;
    public GameObject[] AVE_CapacityAnswer;

}
