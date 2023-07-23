using UnityEngine;
public class AIOAnswerScript : MonoBehaviour {
    // Flag indicating whether the answer is correct or not
    public bool isCorrect = false;
    // Reference to the quiz manager
    public AIOQuizManager quizManager;
    public void Answer() {
        if (isCorrect) {
            // Call the 'correct' method on the quiz manager if the answer is correct
            quizManager.correct();
        } else {
            // Call the 'wrong' method on the quiz manager if the answer is incorrect
            quizManager.wrong();
        }
    }
}
