using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class AIOLength : MonoBehaviour {
    // Public Variables
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
    [Header("Question Audio")]
    public AudioClip[] QAudioClip;
    public AudioClip[] NoAudioClip;
    // Private Variables
    private int correctAns;
    // Easy Private Variables
    private List<int> L_shortRecord = new List<int>();
    private int L_currentQuestionNo;
    private string L_shortObjName;
    // Hard Variable
    private int shortEstimate;
    private AudioSource audioSource;
    private AudioClip LongObjAudio;
    private AudioClip ShortObjAudio;
    private IEnumerator currentQuestionAudioCoroutine = null;
    // Initialize variables and components
    public void callStart() {
        L_currentQuestionNo = 0;
        DurstenfeldShuffle(L_LongObj);
        DurstenfeldShuffle(L_ShortObjOption);
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
    // Set answers for the Easy difficulty level
    public void L_EA_SetAnswers() {
        int[] easyAnswersArr = { correctAns+1, correctAns, correctAns-1 };
        DurstenfeldShuffle(easyAnswersArr);
        
        for (int i = 0; i < L_EA_Options.Length; i++) {
            L_EA_Options[i].GetComponent<AIOAnswerScript>().isCorrect = false;
            L_EA_Options[i].transform.GetChild(0).GetComponent<TMP_Text>().text = easyAnswersArr[i].ToString() + " " + L_shortObjName;
            if (easyAnswersArr[i] == correctAns) {
                L_EA_Options[i].GetComponent<AIOAnswerScript>().isCorrect = true;
            } 
        }
    }
    // Set answers for the Hard difficulty level
    public void L_H_SetAnswers(int currentQuestionNo) {
        int rand_opt = Random.Range(0, 2);
        for (int i = 0; i < L_H_Options.Length; i++) {
            GameObject OptionObjContainer = L_H_Options[i].transform.GetChild(0).gameObject;
            if (OptionObjContainer.transform.childCount > 0) {
                Object.Destroy(OptionObjContainer.transform.GetChild(0).gameObject);
            }
            GameObject GameObj;
            if (rand_opt == 0) {
                GameObj = GameObject.Instantiate(L_LongObj[currentQuestionNo]);
                rand_opt = 1;
            }
            else {
                GameObj = GameObject.Instantiate(L_ShortObjOption[currentQuestionNo]);
                rand_opt = 0;
            }
            GameObj.SetActive(true);
            Instantiate(GameObj, OptionObjContainer.transform);
            
            if (GameObj.GetComponent<ObjectInfo>().ObjectName == L_LongObj[currentQuestionNo].GetComponent<ObjectInfo>().ObjectName) {
                L_H_Options[i].GetComponent<AIOAnswerScript>().isCorrect = true;
            } else {
                L_H_Options[i].GetComponent<AIOAnswerScript>().isCorrect = false;
            }
            StartCoroutine(DieGameObject(GameObj));
        }
        StopQuestionAudio();
    }
    // Method that handles the Question generation for Length Easy and Average level
    public void L_EA_Question(string DIFFICULTY) {
        if (L_EA_ShortCont.transform.childCount > 0) {
            for (var i = L_EA_ShortCont.transform.childCount - 1; i >= 0; i--) {
                Object.Destroy(L_EA_ShortCont.transform.GetChild(i).gameObject);
            }
        }
        if (L_currentQuestionNo != 0) {
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
        while (flag) {
            currShortObject = Random.Range(0, L_ShortObj.Length);
            if (L_shortRecord.Contains(currShortObject) == false) {
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
            // Sets the UI current question for Easy Level
            quizTopUI.Question.text = "How many <color=#ffcb2b>" + L_shortObjName + 
            "</color> are required to equal the length of the <color=#ffcb2b>" + L_LongObj[L_currentQuestionNo].name + "</color>?";
            L_EA_ShortCont.GetComponent<LetCGridLayout>().noMissing = 0;  
        }
        // Average Level
        if (DIFFICULTY == "Average") {
            int noMissing = Random.Range(2, shortEstimate-1);
            L_EA_ShortCont.GetComponent<LetCGridLayout>().noMissing = noMissing; 
            Color c = new Color32(0, 0, 0, 255); 
            GameObject missingShortObj;
            missingShortObj = GameObject.Instantiate(L_ShortObj[currShortObject]);
            missingShortObj.GetComponent<Image>().color = c;
            L_EA_ShortCont.GetComponent<LetCGridLayout>().cellPrefab2 = missingShortObj; 
            correctAns = noMissing;
            // Sets the UI current question for Average Level
            quizTopUI.Question.text = "How many <color=#ffcb2b>missing</color> " + L_shortObjName + 
            " are required to equal the length of the " + L_LongObj[L_currentQuestionNo].name + "?"; 
            StartCoroutine(DieGameObject(missingShortObj));
        }
        // Setup the grid cells
        L_EA_ShortCont.GetComponent<LetCGridLayout>().SetCells();
        LongObjAudio = L_LongObj[L_currentQuestionNo].GetComponent<ObjectInfo>().ObjAudioClip;
        ShortObjAudio = L_ShortObj[currShortObject].GetComponent<ObjectInfo>().ObjAudioClip;
        L_currentQuestionNo +=1;
    }
    // Method that handles the Question generation for Length Hard level
    public void L_H_Question(string DIFFICULTY, int currentQuestionNo) {
        if (L_H_ShortObjCont.transform.childCount > 0) {
            for (var i = L_H_ShortObjCont.transform.childCount - 1; i >= 0; i--) {
                Object.Destroy(L_H_ShortObjCont.transform.GetChild(i).gameObject);
            }
        } 
        // Short Object
        int currShortObject = Random.Range(0, L_ShortObj.Length);
        bool flag = true;
        while (flag) {
            currShortObject = Random.Range(0, L_ShortObj.Length);
            if (L_shortRecord.Contains(currShortObject) == false) {
                L_shortRecord.Add(currShortObject);
                flag = false;
            }
        } 
        shortEstimate = L_LongObj[currentQuestionNo].GetComponent<LongObjectClass>().ShortEstimate[currShortObject];
        L_shortObjName = L_ShortObj[currShortObject].name;
        L_H_ShortObjCont.GetComponent<LetCGridLayout>().col = shortEstimate;
        L_H_ShortObjCont.GetComponent<LetCGridLayout>().cellPrefab = L_ShortObj[currShortObject];
        L_H_ShortObjCont.GetComponent<LetCGridLayout>().SetCells();
        if (DIFFICULTY == "Hard") {
            quizTopUI.Question.text = "Question"; 
            quizTopUI.Question.text = "Which of the following objects has the length of <color=#ffcb2b>" + 
            shortEstimate + " " + L_shortObjName + "</color>?"; 
        }
        LongObjAudio = L_LongObj[currentQuestionNo].GetComponent<ObjectInfo>().ObjAudioClip;
        ShortObjAudio = L_ShortObj[currShortObject].GetComponent<ObjectInfo>().ObjAudioClip;
    }
    // Method for destroying game object in the scene
    IEnumerator DieGameObject(GameObject gameobject){
     yield return new WaitForSeconds(0.1f); //waits 3 seconds
     Object.Destroy(gameobject);
    }
    // Plays or stops the audio for the current question
    public void ToggleQuestionAudio(string DIFFICULTY, int currentQuestionNo) {
        if (currentQuestionAudioCoroutine != null) {
            // If audio is already playing, stop it
            audioSource.Stop();
            StopCoroutine(currentQuestionAudioCoroutine);
            currentQuestionAudioCoroutine = null;
            return;
        } 
        // Start playing the audio for the current question
        currentQuestionAudioCoroutine = QuestionAudio(DIFFICULTY, currentQuestionNo);
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
    public IEnumerator QuestionAudio(string DIFFICULTY, int currentQuestionNo) {
        AudioClip[] clips;
        if (DIFFICULTY == "Easy") { // Audio clips for easy difficulty level
            clips = new AudioClip[] { QAudioClip[0], ShortObjAudio, QAudioClip[1], LongObjAudio };
        } else if (DIFFICULTY == "Average") { // Audio clips for average difficulty level
            clips = new AudioClip[] { QAudioClip[2], ShortObjAudio, QAudioClip[3], LongObjAudio };
        } else { // Audio clips for hard difficulty level
            clips = new AudioClip[] { QAudioClip[4], NoAudioClip[shortEstimate-1], ShortObjAudio };
        }
        foreach (AudioClip clip in clips) { // Play each audio clip in sequence
            audioSource.PlayOneShot(clip);
            yield return new WaitForSeconds(clip.length);
        }
        currentQuestionAudioCoroutine = null;
    }
}