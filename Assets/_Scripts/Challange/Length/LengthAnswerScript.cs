using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LengthAnswerScript : MonoBehaviour
{
    public bool isCorrect = false;
    public LengthQuizManager quizManager;
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