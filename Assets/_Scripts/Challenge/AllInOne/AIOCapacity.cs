using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class AIOCapacity : MonoBehaviour {
    // Public Variables
    [Header("Quiz Top Panel")]
    public QuizTopUI quizTopUI;
    [Header("Game Objects")]
    public GameObject[] CapacityObjects;
    public GameObject[] aveCapacityObjects;
    public GameObject[] hardCapacityObjects;
    public GameObject[] hardSmallCapacityObjects;
    public GameObject[] AVE_CapacitySlots;
    [Header("Quiz Options")]
    public GameObject[] Options;
    public GameObject SubmitButton;
    public GameObject[] HardOptions;
    [Header("Game Object Container")]
    public GameObject[] EasyContainer;
    public GameObject[] AverContainer;
    [Header("Hard Grid")]
    public GameObject QuestionObject;
    public GameObject QuestionAGrid;
    public GameObject QuestionBGrid;
    [Header("Question Audio")]
    public AudioClip[] QAudioClip;
    public AudioClip[] NoAudioClip;
    public int currentQuestionNo;
    // Private Variables
    private AudioSource audioSource;
    private AudioClip Obj1Audio;
    private AudioClip Obj2Audio;
    private AudioClip No1Audio;
    private AudioClip No2Audio;
    List<int> arrRecord = new List<int>();
    private List<int> smallRecord = new List<int>();
    private string firstObjName, secondObjName;
    private int left, right, correctAns;
    private string smallHardObjName;
    private int currHardSmall, currHardObjNo, multiplier;
    private IEnumerator currentQuestionAudioCoroutine = null;
    // Initialize variables and components
    public void callStart() {
        DurstenfeldShuffle(CapacityObjects);
        DurstenfeldShuffle(aveCapacityObjects);
        DurstenfeldShuffle(hardCapacityObjects);
        audioSource = GetComponent<AudioSource>();
    }
    // Shuffle an array using the Durstenfeld algorithm
    public void DurstenfeldShuffle<T>(T[] gameObjectArr) {
        int last_index = gameObjectArr.Length - 1;
        while (last_index > 0) {
            int rand_index = Random.Range(0, last_index+1);
            T temp = gameObjectArr[last_index];
            gameObjectArr[last_index] = gameObjectArr[rand_index];
            gameObjectArr[rand_index] = temp;
            last_index -= 1;
        }
    }
    // Set Answers for all level of difficulty
    public void SetAnswers(string DIFFICULTY) {
        if (DIFFICULTY == "Easy") { // Set Answers for Easy level of difficulty
            string[] choice = {"MORE", "LESS"};
            DurstenfeldShuffle(Options);
            // Iterate through Options array and set text for options
            for (int i = 0; i < Options.Length; i++) {
                Options[i].GetComponent<AIOAnswerScript>().isCorrect = false;
                Options[0].transform.GetChild(0).GetComponent<TMP_Text>().text = choice[0];
                Options[1].transform.GetChild(0).GetComponent<TMP_Text>().text = choice[1];
            } 
            if (left < right) { // Compare left and right values and set correct answer
                Options[1].GetComponent<AIOAnswerScript>().isCorrect = true;
            } else if (left > right) {
                Options[0].GetComponent<AIOAnswerScript>().isCorrect = true;
            }
        } 
        if (DIFFICULTY == "Average") { // Set Answers for Average level of difficulty
            SubmitButton.transform.GetChild(0).GetComponent<TMP_Text>().text = "Submit";
        } 
        if (DIFFICULTY == "Hard") { // Set Answers for Hard level of difficulty
            int[] hardAnswers = new int[2];
            string sign = (UnityEngine.Random.value > 0.5f) ? "-" : "+";
            // Assign values to hardAnswers based on the sign and multiplier
            if (sign == "-" && ((multiplier - 1) != 0)) {
                hardAnswers[0] = multiplier - 1;
                hardAnswers[1] = multiplier;
            } else {
                hardAnswers[0] = multiplier + 1;
                hardAnswers[1] = multiplier;
            }
            DurstenfeldShuffle(hardAnswers); 
            // Iterate through HardOptions array
            for (int i = 0; i < HardOptions.Length; i++) {
                if (HardOptions[i] != null) {
                    HardOptions[i].GetComponent<AIOAnswerScript>().isCorrect = false;
                    GameObject OptionGrid = HardOptions[i].transform.GetChild(0).gameObject;
                    // Destroy child objects in grid
                    if (OptionGrid.transform.childCount > 0) {
                        for (var j = OptionGrid.transform.childCount - 1; j >= 0; j--) {
                            Object.Destroy(OptionGrid.transform.GetChild(j).gameObject);
                        }
                    }
                    // Setup grid based on currHardObjNo
                    OptionGrid.GetComponent<CapacityGridLayout>().row = currHardObjNo;
                    switch (currHardObjNo) {
                        case 2:
                            OptionGrid.GetComponent<CapacityGridLayout>().columns = new int[] {
                                hardAnswers[i], hardAnswers[i]};
                            break;
                        case 3:
                            OptionGrid.GetComponent<CapacityGridLayout>().columns = new int[] {
                                hardAnswers[i], hardAnswers[i], hardAnswers[i]};
                            break;
                        case 4:
                            OptionGrid.GetComponent<CapacityGridLayout>().columns = new int[] {
                                hardAnswers[i], hardAnswers[i], hardAnswers[i], hardAnswers[i]};
                            break;
                        default:
                            OptionGrid.GetComponent<CapacityGridLayout>().columns = new int[] {
                                hardAnswers[i], hardAnswers[i], hardAnswers[i], hardAnswers[i], hardAnswers[i]};
                            break;
                    }
                    OptionGrid.GetComponent<CapacityGridLayout>().cellPrefab = hardSmallCapacityObjects[currHardSmall];
                    OptionGrid.GetComponent<CapacityGridLayout>().SetCells();
                    // multiplier * currHardObjNo
                    if (hardAnswers[i] * currHardObjNo == correctAns) {
                        HardOptions[i].GetComponent<AIOAnswerScript>().isCorrect = true;
                    } 
                }
            }
        } 
    }
    // Handler for Submit button in Average level
    public void AverageSubmit() {
        int first_iden = 0; // Variable to store the capacity of the first identified item
        int second_iden = 0; // Variable to store the capacity of the second identified item
        int third_iden = 0; // Variable to store the capacity of the third identified item
        // Iterate through AverContainer array
        for (int i = 0; i < AverContainer.Length; i++) {
            if (Mathf.Round(AverContainer[i].transform.GetChild(0).position.x) == 
            Mathf.Round(AVE_CapacitySlots[0].transform.position.x)){
                first_iden = AverContainer[i].transform.GetChild(0).GetComponent<ItemCapacity>().capacity;
            }
            else if (Mathf.Round(AverContainer[i].transform.GetChild(0).position.x) == 
            Mathf.Round(AVE_CapacitySlots[1].transform.position.x)) {
                second_iden = AverContainer[i].transform.GetChild(0).GetComponent<ItemCapacity>().capacity;
            }
            else if (Mathf.Round(AverContainer[i].transform.GetChild(0).position.x) == 
            Mathf.Round(AVE_CapacitySlots[2].transform.position.x)) {
                third_iden = AverContainer[i].transform.GetChild(0).GetComponent<ItemCapacity>().capacity;
            }
        } 
        // Check the conditions for correctness
        if (first_iden < second_iden && third_iden > second_iden) { // Set Submit button as correct answer
            SubmitButton.GetComponent<AIOAnswerScript>().isCorrect = true; 
        } else { // Set Submit button as correct answer
            SubmitButton.GetComponent<AIOAnswerScript>().isCorrect = false; 
        }
    }
    // Method that handles the Question generation for Capacity Easy level
    public void EasyQuestion() {
        // Reference to the 1st & 2nd container in the EasyContainer array
        GameObject firstContainer = EasyContainer[0];
        GameObject secondContainer = EasyContainer[1];
        // Destroy any existing child objects in the containers
        if (EasyContainer[0].transform.childCount > 0) {
            Object.Destroy(EasyContainer[0].transform.GetChild(0).gameObject);
            Object.Destroy(EasyContainer[1].transform.GetChild(0).gameObject);
        }
        // Generate a random index for the 1st & 2nd object
        int currFirstObject = Random.Range(0, CapacityObjects.Length);
        int currSecondObject = Random.Range(0, CapacityObjects.Length);
        bool flag = true;
        // Loop to ensure unique objects are selected and objects with the same capacity are not chosen
        while (flag) {
            currFirstObject = Random.Range(0, CapacityObjects.Length);
            currSecondObject = Random.Range(0, CapacityObjects.Length);
             // Check if the first object index is not in the arrRecord
            if (arrRecord.Contains(currFirstObject) == false) {
                arrRecord.Add(currFirstObject);
                flag = false;
            }
             // Check if the second object index is not in the arrRecord
            if (arrRecord.Contains(currSecondObject) == false) {
                arrRecord.Add(currSecondObject);
                flag = false;
            }
            // Check if the capacities of the selected objects are the same
            if (CapacityObjects[currFirstObject].transform.GetComponent<ItemCapacity>().capacity == 
            CapacityObjects[currSecondObject].transform.GetComponent<ItemCapacity>().capacity) {
                arrRecord.Remove(currSecondObject);
                flag = true;
            }
        }
        // Retrieve object names and capacities for the chosen objects
        firstObjName = CapacityObjects[currFirstObject].GetComponent<LabelScriptClass>().objLabel;
        left = CapacityObjects[currFirstObject].GetComponent<ItemCapacity>().capacity;
        secondObjName = CapacityObjects[currSecondObject].GetComponent<LabelScriptClass>().objLabel;
        right = CapacityObjects[currSecondObject].GetComponent<ItemCapacity>().capacity;
        // Reference to the left and right object game object
        GameObject leftObject = CapacityObjects[currFirstObject];
        GameObject rightObject = CapacityObjects[currSecondObject];
        // Instantiate the left and right objects into their respective containers
        Instantiate(leftObject, firstContainer.transform);
        Instantiate(rightObject, secondContainer.transform);
        // Sets the UI current question for Easy Level
        quizTopUI.Question.text = "Does the <color=#ffcb2b>" + firstObjName + 
        "</color> hold more or less than the <color=#ffcb2b>" + secondObjName + "</color>?";
        Obj1Audio = CapacityObjects[currFirstObject].GetComponent<LabelScriptClass>().ObjAudioClip;
        Obj2Audio = CapacityObjects[currSecondObject].GetComponent<LabelScriptClass>().ObjAudioClip;
    }
    // Method that handles the Question generation for Capacity Average level
    public void AverageQuestion(){
        // Sets the UI current question for Average Level
        quizTopUI.Question.text = "Arrange the objects from <color=#ffcb2b>least</color> to <color=#ffcb2b>greatest</color> capacity";
        // Destroy any existing child objects in the AverContainer
        if (AverContainer[0].transform.childCount > 0) {
            Object.Destroy(AverContainer[0].transform.GetChild(0).gameObject);
            Object.Destroy(AverContainer[1].transform.GetChild(0).gameObject);
            Object.Destroy(AverContainer[2].transform.GetChild(0).gameObject);
        }
        arrRecord.Clear(); // Clear the arrRecord list used to keep track of chosen objects
        for (int i = 0; i < 3; i++) {
            int currObject = Random.Range(0, aveCapacityObjects.Length);
            bool flag = true;
            while (flag) {
                currObject = Random.Range(0, aveCapacityObjects.Length);
                // Check if the current object index is not in the arrRecord
                if (arrRecord.Contains(currObject) == false) {
                    arrRecord.Add(currObject);
                    flag = false;
                }
            }
            // Instantiate the current object into the AverContainer
            Instantiate(aveCapacityObjects[currObject], AverContainer[i].transform);
        }
    }
    // Method that handles the Question generation for Capacity Hard level
    public void HardQuestion() {
        // Destroy any existing child objects in the QuestionObject
        if (QuestionObject.transform.childCount > 0) {
            Object.Destroy(QuestionObject.transform.GetChild(0).gameObject);
        }
        // Destroy child objects in QuestionAGrid
        if (QuestionAGrid.transform.childCount > 0) {
            for (var i = QuestionAGrid.transform.childCount - 1; i >= 0; i--) {
                Object.Destroy(QuestionAGrid.transform.GetChild(i).gameObject);
            }
        }
        // Destroy child objects in QuestionBGrid
        if (QuestionBGrid.transform.childCount > 0) {
            for (var i = QuestionBGrid.transform.childCount - 1; i >= 0; i--) {
                Object.Destroy(QuestionBGrid.transform.GetChild(i).gameObject);
            }
        }
        GameObject MainObject;
        MainObject = GameObject.Instantiate(hardCapacityObjects[currentQuestionNo], QuestionObject.transform);
        // Adjust the size of the MainObject to fit the QuestionObject's dimensions
        RectTransform _rTransform = MainObject.GetComponent<RectTransform>();
        float _rWidth = QuestionObject.GetComponent<RectTransform>().rect.width;
        float _rHeight = QuestionObject.GetComponent<RectTransform>().rect.height;
        _rTransform.sizeDelta = new Vector2(_rWidth, _rHeight);
        currHardSmall = Random.Range(0, 6); // select random small object
        multiplier = hardCapacityObjects[currentQuestionNo].GetComponent<MultiplierClassScript>().multiplier[currHardSmall];
        // Set the layout for QuestionAGrid based on the multiplier
        if (multiplier > 5) {
            int tmp = Mathf.FloorToInt(multiplier/2);
            QuestionAGrid.GetComponent<CapacityGridLayout>().row = 2;
            QuestionAGrid.GetComponent<CapacityGridLayout>().columns = new int[] {multiplier-tmp, tmp};
        } else {
            QuestionAGrid.GetComponent<CapacityGridLayout>().row = 1;
            QuestionAGrid.GetComponent<CapacityGridLayout>().columns = new int[] {multiplier};
        }
        // Setup cells with small game objects for Queastion A Grid Container
        QuestionAGrid.GetComponent<CapacityGridLayout>().cellPrefab = hardSmallCapacityObjects[currHardSmall];
        QuestionAGrid.GetComponent<CapacityGridLayout>().SetCells();
        // Setup cells with small game objects for Queastion B Grid Container
        currHardObjNo = Random.Range(2, 4);
        QuestionBGrid.GetComponent<CapacityGridLayout>().columns = new int[] {currHardObjNo};
        QuestionBGrid.GetComponent<CapacityGridLayout>().cellPrefab = hardCapacityObjects[currentQuestionNo];
        QuestionBGrid.GetComponent<CapacityGridLayout>().SetCells();
        // Get names of objects for setting up the text question
        smallHardObjName = hardSmallCapacityObjects[currHardSmall].GetComponent<LabelScriptClass>().objLabel;
        correctAns = multiplier * currHardObjNo;
        firstObjName = hardCapacityObjects[currentQuestionNo].GetComponent<LabelScriptClass>().objLabel;
        secondObjName = hardSmallCapacityObjects[currHardSmall].GetComponent<LabelScriptClass>().objLabel;
        // Sets the UI current question for Hard Level
        quizTopUI.Question.text = multiplier + " " + secondObjName + " are needed to fill 1 " + firstObjName + 
        ". How many <color=#ffcb2b>" + secondObjName + "</color> are needed to fill <color=#ffcb2b>" + currHardObjNo + 
        " " + firstObjName + "</color>." ;
        // Setup the current audio of the current objects for the question reading
        Obj1Audio = hardCapacityObjects[currentQuestionNo].GetComponent<LabelScriptClass>().ObjAudioClip;
        Obj2Audio = hardSmallCapacityObjects[currHardSmall].GetComponent<LabelScriptClass>().ObjAudioClip;
        No1Audio = NoAudioClip[multiplier-1];
        No2Audio = NoAudioClip[currHardObjNo-1];
        currentQuestionNo += 1 ;
    }
    // Plays or stops the audio for the current question
    public void ToggleQuestionAudio(string DIFFICULTY) {
        if (currentQuestionAudioCoroutine != null) {
            // If audio is already playing, stop it
            audioSource.Stop();
            StopCoroutine(currentQuestionAudioCoroutine);
            currentQuestionAudioCoroutine = null;
            return;
        }
        // Start playing the audio for the current question
        currentQuestionAudioCoroutine = QuestionAudio(DIFFICULTY);
        StartCoroutine(currentQuestionAudioCoroutine);
    }
    // Stops current question audio
    public void StopQuestionAudio() {
        if (currentQuestionAudioCoroutine != null) {
            // Stop the audio and coroutine
            audioSource.Stop();
            StopCoroutine(currentQuestionAudioCoroutine);
            currentQuestionAudioCoroutine = null;
            return;
        }
    }
    // Coroutine to play the audio for the current question
    public IEnumerator QuestionAudio(string DIFFICULTY) {
        AudioClip[] clips;
        if (DIFFICULTY == "Easy") { // Audio clips for easy difficulty level
            clips = new AudioClip[] { QAudioClip[0], Obj1Audio, QAudioClip[1], Obj2Audio };
        } else if (DIFFICULTY == "Average") { // Audio clips for average difficulty level
            clips = new AudioClip[] { QAudioClip[2] };
        } else { // Audio clips for hard difficulty level
            clips = new AudioClip[] { No1Audio, Obj2Audio, QAudioClip[3], NoAudioClip[0], Obj1Audio, 
            QAudioClip[4], Obj2Audio, QAudioClip[3], No2Audio, Obj1Audio };
        }
        foreach (AudioClip clip in clips) { // Play each audio clip in sequence
            audioSource.PlayOneShot(clip);
            yield return new WaitForSeconds(clip.length);
        }
        currentQuestionAudioCoroutine = null;
    }
}
