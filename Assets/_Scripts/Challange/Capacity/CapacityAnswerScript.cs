using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapacityAnswerScript : MonoBehaviour
{
    public bool isCorrect = false;
    public CapacityQuizManager quizManager;
    public void Answer() 
    {
        if (isCorrect) 
        {
            quizManager.correct();
        }
        else
        {
            quizManager.wrong();
        }
    }
}