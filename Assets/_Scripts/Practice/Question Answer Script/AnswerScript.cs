using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerScript : MonoBehaviour
{
    public bool isCorrect = false;
    public PracticeQuizManager quizManager;
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
