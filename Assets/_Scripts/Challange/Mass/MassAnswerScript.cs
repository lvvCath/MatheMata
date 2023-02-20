using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MassAnswerScript : MonoBehaviour
{
    public bool isCorrect = false;
    public MassQuizManager quizManager;
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