using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIOAnswerScript : MonoBehaviour
{
    public bool isCorrect = false;
    public AIOQuizManager quizManager;
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
