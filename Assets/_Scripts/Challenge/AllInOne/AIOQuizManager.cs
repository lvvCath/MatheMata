using System.Collections;
using UnityEngine;
public class AIOQuizManager : MonoBehaviour {
    // Public Variables
    [Header("Quiz Info")]
    public string CATEGORY;
    public string DIFFICULTY;
    public int setCategory;
    public int setDifficulty;
    [Header("Quiz Top Panel")]
    public QuizTopUI quizTopUI;
    [Header("Quiz Values")]
    public int currentQuestionNo;
    public int total;
    public float timeLimit;
    [Header("PopUp Overlay Panel")]
    public GameObject WrongOverlay;
    public GameObject CorrectOverlay;
    public GameObject ResultPanel;
    [Header("Audio SFX")]
    public AudioSource correctSFX;
    public AudioSource wrongSFX;
    [Header("Question Content Container")]
    public GameObject L_EasyAveCont;
    public GameObject L_HardCont;
    public GameObject M_EasyCont;
    public GameObject M_AveCont;
    public GameObject M_HardCont;
    public GameObject M_OptionCont;
    public GameObject C_EasyCont;
    public GameObject C_AveCont;
    public GameObject C_HardCont;
    [Header("Quiz Manager")]
    public GameObject QUIZMANAGER;
    // Private Variables
    private int score;
    private float timer;
    private bool stopTimer;

    private void Start() {
        // Set the category text in the UI
        quizTopUI.Category.text = QuizData.CATEGORY;
        CATEGORY = QuizData.CATEGORY;
        DIFFICULTY = QuizData.DIFFICULTY;
        // Check if the category is not "All In One"
        if (CATEGORY  != "All In One") {
            string[] categories = {"Length", "Mass", "Capacity"};
            string[] difficulties = {"Easy", "Average", "Hard"};
            // Set the category and difficulty based on the selected values
            setCategory = System.Array.IndexOf(categories, CATEGORY) + 1;
            setDifficulty = System.Array.IndexOf(difficulties, DIFFICULTY) + 1;
            // Set the quiz result panel with the selected category and difficulty
            ResultPanel.GetComponent<QuizResultAnim>().setQuiz(CATEGORY, DIFFICULTY);
            // Set the difficulty text in the UI
            quizTopUI.Difiiculty.text = DIFFICULTY;
        } else {
            // Set the quiz result panel for "All In One" category
            ResultPanel.GetComponent<QuizResultAnim>().setQuiz("All In One", "");
        }
        // Call the Start() method for each quiz category
        QUIZMANAGER.GetComponent<AIOLength>().callStart();
        QUIZMANAGER.GetComponent<AIOMass>().callStart();
        QUIZMANAGER.GetComponent<AIOCapacity>().callStart();
        // Generate the first question
        GenerateQuestion();
    }

    private void Update() {
        // Check if the timer should continue running
        if(stopTimer == false) {
            // Decrease the timer
            timer -= Time.deltaTime;
            // Update the timer text in minutes and seconds format
            int minutes = Mathf.FloorToInt(timer / 60);
            int seconds = Mathf.FloorToInt(timer % 60);
            quizTopUI.Timer.text = minutes.ToString("00") + ":" + seconds.ToString("00");
            // Update the timer value in the slider
            quizTopUI.TimerSlider.value = timer;
            if (timer <= 0) { // Check if the timer has reached 0
                GenerateQuestion(); // Generate a new question
            }
        }
    }

    private void GenerateQuestion() {
        // Check if there are more questions to generate
        if (currentQuestionNo < total) {
            HideContainers(); // Hide question containers
            // Update the question number in the UI
            quizTopUI.QuestionNo.text = "Question " + (currentQuestionNo+1).ToString();
            // Check if the category is "All In One" and generate random category and difficulty
            if (CATEGORY == "All In One") {
                setCategory = Random.Range(1, 4); // (1) Length, (2) Mass, (3) Capacity
                setDifficulty = Random.Range(1, 4);  // (1) Easy, (2) Average, (3) Hard
            } 
            // Generate question based on the selected category and difficulty
            switch (setCategory) {
                case 1: // Length category
                    if (CATEGORY == "All In One") {
                        quizTopUI.Difiiculty.text = "Length";
                    }
                    switch (setDifficulty) {
                        case 1: // Easy difficulty
                            DIFFICULTY = "Easy";
                            timeLimit = 90;
                            L_EasyAveCont.SetActive(true);
                            QUIZMANAGER.GetComponent<AIOLength>().L_EA_Question(DIFFICULTY);
                            QUIZMANAGER.GetComponent<AIOLength>().L_EA_SetAnswers();
                            break;
                        case 2: // Average difficulty
                            DIFFICULTY = "Average";
                            timeLimit = 60;
                            L_EasyAveCont.SetActive(true);
                            QUIZMANAGER.GetComponent<AIOLength>().L_EA_Question(DIFFICULTY);
                            QUIZMANAGER.GetComponent<AIOLength>().L_EA_SetAnswers();
                            break;
                        case 3: // Hard difficulty
                            DIFFICULTY = "Hard";
                            timeLimit = 30;
                            L_HardCont.SetActive(true);
                            QUIZMANAGER.GetComponent<AIOLength>().L_H_Question(DIFFICULTY, currentQuestionNo);
                            QUIZMANAGER.GetComponent<AIOLength>().L_H_SetAnswers(currentQuestionNo);
                            break;
                        default:
                            Debug.LogError("Invalid difficulty level: " + setDifficulty);
                            break;
                    }
                    break;
                case 2:  // Mass  category
                    if (CATEGORY == "All In One") {
                        quizTopUI.Difiiculty.text = "Mass";
                    }
                    switch (setDifficulty) {
                        case 1: // Easy difficulty
                            DIFFICULTY = "Easy";
                            timeLimit = 60;
                            M_EasyCont.SetActive(true);
                            M_OptionCont.SetActive(true);
                            QUIZMANAGER.GetComponent<AIOMass>().EasyQuestion();
                            break;
                        case 2: // Average difficulty
                            DIFFICULTY = "Average";
                            timeLimit = 60;
                            M_AveCont.SetActive(true);
                            M_OptionCont.SetActive(true);
                            QUIZMANAGER.GetComponent<AIOMass>().AverageQuestion();
                            break;
                        case 3: // Hard difficulty
                            DIFFICULTY = "Hard";
                            timeLimit = 90;
                            M_HardCont.SetActive(true);
                            QUIZMANAGER.GetComponent<AIOMass>().HardQuestion();
                            break;
                        default:
                            Debug.LogError("Invalid difficulty level: " + setDifficulty);
                            break;
                    }
                    QUIZMANAGER.GetComponent<AIOMass>().SetAnswers(DIFFICULTY);
                    break;
                case 3: // Capacity category
                    if (CATEGORY == "All In One") {
                        quizTopUI.Difiiculty.text = "Capacity";
                    }
                    switch (setDifficulty) {
                        case 1: // Easy difficulty
                            DIFFICULTY = "Easy";
                            timeLimit = 30;
                            C_EasyCont.SetActive(true);
                            QUIZMANAGER.GetComponent<AIOCapacity>().EasyQuestion();
                            break;
                        case 2: // Average difficulty
                            DIFFICULTY = "Average";
                            timeLimit = 60;
                            C_AveCont.SetActive(true);
                            QUIZMANAGER.GetComponent<AIOCapacity>().AverageQuestion();
                            break;
                        case 3: // Hard difficulty
                            DIFFICULTY = "Hard";
                            timeLimit = 120;
                            C_HardCont.SetActive(true);
                            QUIZMANAGER.GetComponent<AIOCapacity>().HardQuestion();
                            break;
                        default:
                            Debug.LogError("Invalid difficulty level: " + setDifficulty);
                            break;
                    }
                    QUIZMANAGER.GetComponent<AIOCapacity>().SetAnswers(DIFFICULTY);
                    break;
                default:
                    Debug.LogError("Invalid category: " + setCategory);
                    break;
            }
            // Reset timer and update UI
            stopTimer = false;
            quizTopUI.TimerSlider.maxValue = timeLimit;
            quizTopUI.TimerSlider.value = timeLimit;
            timer = timeLimit;
            currentQuestionNo += 1;
        }
        else {
            // Timer
            stopTimer = true;
            quizTopUI.Timer.text = "00:00";
            // Display Result Panel
            ResultPanel.SetActive(true);
            ResultPanel.GetComponent<QuizResultAnim>().setScore(score.ToString(), total.ToString());
        }
    }

    IEnumerator nextQuestion(GameObject guiParentCanvas, float secondsToWait, string answer) {
        yield return new WaitForSeconds(secondsToWait);
        guiParentCanvas.GetComponent<OverlayPanel>().CloseOverlay();
        GenerateQuestion();
    }
    // Method called when the answer is correct
    public void correct() {
        score += 1;
        stopTimer = true; // Timer 
        StopQuestionAudio();
        quizTopUI.Score.text = (score).ToString() + " / " + total.ToString();
        CorrectOverlay.SetActive(true);
        correctSFX.Play();
        StartCoroutine(nextQuestion(CorrectOverlay, 2.0f, "correct"));
    }
    // Method called when the answer is wrong
    public void wrong() {
        stopTimer = true; // Timer
        StopQuestionAudio();
        WrongOverlay.SetActive(true);
        wrongSFX.Play();
        StartCoroutine(nextQuestion(WrongOverlay, 2.0f, "wrong"));
    }
    // Method to hide UI containers
    private void HideContainers() {
        L_EasyAveCont.SetActive(false);
        L_HardCont.SetActive(false);
        M_EasyCont.SetActive(false);
        M_AveCont.SetActive(false);
        M_HardCont.SetActive(false);
        M_OptionCont.SetActive(false);
        C_EasyCont.SetActive(false);
        C_AveCont.SetActive(false);
        C_HardCont.SetActive(false);
    }
    // Method to Play the audio for the current question
    public void PlayQuestion() {
        switch (setCategory) {
            case 1:
                switch (setDifficulty) {
                    case 1:
                        DIFFICULTY = "Easy";
                        QUIZMANAGER.GetComponent<AIOLength>().ToggleQuestionAudio(DIFFICULTY, currentQuestionNo);
                        break;
                    case 2:
                        DIFFICULTY = "Average";
                        QUIZMANAGER.GetComponent<AIOLength>().ToggleQuestionAudio(DIFFICULTY, currentQuestionNo);
                        break;
                    case 3:
                        DIFFICULTY = "Hard";
                        QUIZMANAGER.GetComponent<AIOLength>().ToggleQuestionAudio(DIFFICULTY, currentQuestionNo);
                        break;
                    default:
                        Debug.LogError("Invalid difficulty level: " + setDifficulty);
                        break;
                }
                break;
            case 2:
                switch (setDifficulty) {
                    case 1:
                        DIFFICULTY = "Easy";
                        QUIZMANAGER.GetComponent<AIOMass>().ToggleQuestionAudio(DIFFICULTY);
                        break;
                    case 2:
                        DIFFICULTY = "Average";
                        QUIZMANAGER.GetComponent<AIOMass>().ToggleQuestionAudio(DIFFICULTY);
                        break;
                    case 3:
                        DIFFICULTY = "Hard";
                        QUIZMANAGER.GetComponent<AIOMass>().ToggleQuestionAudio(DIFFICULTY);
                        break;
                    default:
                        Debug.LogError("Invalid difficulty level: " + setDifficulty);
                        break;
                }
                break;
            case 3:
                switch (setDifficulty) {
                    case 1:
                        DIFFICULTY = "Easy";
                        QUIZMANAGER.GetComponent<AIOCapacity>().ToggleQuestionAudio(DIFFICULTY);
                        break;
                    case 2:
                        DIFFICULTY = "Average";
                        QUIZMANAGER.GetComponent<AIOCapacity>().ToggleQuestionAudio(DIFFICULTY);
                        break;
                    case 3:
                        DIFFICULTY = "Hard";
                        QUIZMANAGER.GetComponent<AIOCapacity>().ToggleQuestionAudio(DIFFICULTY);
                        break;
                    default:
                        Debug.LogError("Invalid difficulty level: " + setDifficulty);
                        break;
                }
                break;
            default:
                Debug.LogError("Invalid category: " + setCategory);
                break;
        }
    }
    // Method to stop current question audio
    private void StopQuestionAudio() {
        switch (setCategory) {
            case 1:
                QUIZMANAGER.GetComponent<AIOLength>().StopQuestionAudio();
                break;
            case 2:
                QUIZMANAGER.GetComponent<AIOMass>().StopQuestionAudio();
                break;
            case 3:
                QUIZMANAGER.GetComponent<AIOCapacity>().StopQuestionAudio();
                break;
            default:
                Debug.LogError("Invalid category: " + setCategory);
                break;
        }
    }
}