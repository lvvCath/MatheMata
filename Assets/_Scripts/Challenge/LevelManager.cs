using UnityEngine;
public class LevelManager : MonoBehaviour {
    // import script
    QuizSceneManager quizManager;
    // Public Variables
    [Header("Canvas")]
    public GameObject CategoryCanvas;
    public GameObject DifficultCanvas;
    public GameObject LockMessagePanel;
    public GameObject DifficultyBtnPanel;
    public GameObject[] CategoryText;
    [Header("Buttons")]
    public GameObject MainMenuBtn;
    public GameObject NavigationBtn;
    public GameObject[] CategoryButtons;
    public GameObject[] DifficultyButtons;
    [Header("Button Content")]
    public GameObject[] CategoryLocks;
    public GameObject[] CategoryContent;
    public GameObject[] DifficultyLocks;
    public GameObject[] DifficultyContent;
    [Header("All in One Content")]
    public GameObject AOIButton;
    public GameObject AOIStars;
    public GameObject AOILock;
    // Private Variables
    int[] Length = { 0, 0, 0 };
    int[] Mass = { 0, 0, 0 };
    int[] Capacity = { 0, 0, 0 };
    // Define arrays for categories, difficulties, score text objects, total score, and total items
    string[] categories = { "Length", "Mass", "Capacity" };
    string[] difficulties = { "Easy", "Average", "Hard" };
    int CategoryLock = 0;
    int DifficultyLock = 0;
    // true = Lock | false = Unlocked
    bool[] lockCategory = { false, true, true };
    bool[] lockLength = { false, true, true };
    bool[] lockMass = { true, true, true };
    bool[] lockCapacity = { true, true, true };
    int[] TotalScore = { 0, 0, 0 };
    int[] TotalItems = { 0, 0, 0 };
    // Start is called before the first frame update
    void Start() {
        for (int i = categories.Length-1; i >= 0 ; i--) {
            switch (i) {
                case 0: // Length
                    GetScore(i, difficulties, Length);
                    break;
                case 1: // Mass
                    GetScore(i, difficulties, Mass);
                    break;
                case 2: // Capacity
                    GetScore(i, difficulties, Capacity);
                    break;
            }
        }
        // Check category and difficulty locks and set the corresponding game object states
        switch(CategoryLock) {
            case 0:
                if (DifficultyLock == 0) {
                    AOILock.SetActive(false);
                    AOIStars.SetActive(true);
                    AOIButton.GetComponent<LevelLock>().isLocked = false;
                } else {
                    AOILock.SetActive(true);
                    AOIStars.SetActive(false);
                    AOIButton.GetComponent<LevelLock>().isLocked = true;
                    CategoryButtons[0].GetComponent<LevelLock>().isLocked = false;
                    CategoryButtons[1].GetComponent<LevelLock>().isLocked = true;
                    CategoryButtons[2].GetComponent<LevelLock>().isLocked = true;
                    CategoryLocks[0].SetActive(false);
                    CategoryContent[0].SetActive(true);
                    CategoryLocks[1].SetActive(true);
                    CategoryContent[1].SetActive(false);
                    CategoryLocks[2].SetActive(true);
                    CategoryContent[2].SetActive(false);
                }
                break;
            case 1:
                AOILock.SetActive(true);
                AOIStars.SetActive(false);
                AOIButton.GetComponent<LevelLock>().isLocked = true;
                CategoryButtons[0].GetComponent<LevelLock>().isLocked = false;
                CategoryButtons[1].GetComponent<LevelLock>().isLocked = false;
                CategoryButtons[2].GetComponent<LevelLock>().isLocked = true;
                CategoryLocks[0].SetActive(false);
                CategoryContent[0].SetActive(true);
                CategoryLocks[1].SetActive(false);
                CategoryContent[1].SetActive(true);
                CategoryLocks[2].SetActive(true);
                CategoryContent[2].SetActive(false);
                break;  
            case 2:
                AOILock.SetActive(true);
                AOIStars.SetActive(false);
                AOIButton.GetComponent<LevelLock>().isLocked = true;
                CategoryButtons[0].GetComponent<LevelLock>().isLocked = false;
                CategoryButtons[1].GetComponent<LevelLock>().isLocked = false;
                CategoryButtons[2].GetComponent<LevelLock>().isLocked = false;
                CategoryLocks[0].SetActive(false);
                CategoryContent[0].SetActive(true);
                CategoryLocks[1].SetActive(false);
                CategoryContent[1].SetActive(true);
                CategoryLocks[2].SetActive(false);
                CategoryContent[2].SetActive(true);
                break;
        }
    }
    // Calculate the score and total items for a specific category and difficulty
    void GetScore(int categoryIndex, string[] difficulties, int[] levelScores) {
        int totalScore = 0;
        int totalItems = 0;
        for (int j = difficulties.Length-1; j >= 0; j--) {
            string scoreKey = categories[categoryIndex] + " " + difficulties[j] + " Score";
            string totalKey = categories[categoryIndex] + " " + difficulties[j] + " Total";
            int scoreValue = PlayerPrefs.GetInt(scoreKey, 0);
            int totalValue = PlayerPrefs.GetInt(totalKey, 10);
            levelScores[j] = scoreValue;
            totalScore += scoreValue;
            totalItems += totalValue;
            if (scoreValue < Mathf.FloorToInt(( (75 * totalValue) / 100 )) ) { 
                CategoryLock = categoryIndex;
                DifficultyLock = j+1;
            }   
        }
        TotalScore[categoryIndex] = totalScore;
        TotalItems[categoryIndex] = totalItems;
    }
    // Button click handler for the Length category
    public void clickLength() {
        DifficultCanvas.SetActive(true);
        lockDifficulty(0);
    }
    // Button click handler for the Mass category
    public void clickMass() {
        if (!CategoryButtons[1].GetComponent<LevelLock>().isLocked) {
            DifficultCanvas.SetActive(true);
            CategoryCanvas.SetActive(false);
            DifficultyBtnPanel.SetActive(true);
            CategoryText[1].SetActive(true);
            MainMenuBtn.SetActive(false);
            NavigationBtn.SetActive(false);
            lockDifficulty(1);
        } else {
            LockMessagePanel.SetActive(true);
        }
    }
    // Button click handler for the Capacity category
    public void clickCapacity() {
        if (!CategoryButtons[2].GetComponent<LevelLock>().isLocked) {
            DifficultCanvas.SetActive(true);
            CategoryCanvas.SetActive(false);
            DifficultyBtnPanel.SetActive(true);
            CategoryText[2].SetActive(true);
            MainMenuBtn.SetActive(false);
            NavigationBtn.SetActive(false);
            lockDifficulty(2);
        } else {
            LockMessagePanel.SetActive(true);
        }
    }
    // Lock or unlock the difficulty buttons based on the selected category
    private void lockDifficulty (int selectedCategory) {
        if (CategoryLock == selectedCategory) {
            switch(DifficultyLock) {
            case 1:
                for (int i = 0; i < DifficultyLocks.Length; i++) {
                    DifficultyLocks[i].SetActive(i != 0);
                    DifficultyContent[i].SetActive(i == 0);
                    if (i != 0) {
                        DifficultyButtons[i].GetComponent<LevelLock>().isLocked = true;
                    } else if (i == 0) {
                        DifficultyButtons[i].GetComponent<LevelLock>().isLocked = false;
                    }
                }
                break;
            case 2:
                for (int i = 0; i < DifficultyLocks.Length; i++) {
                    DifficultyLocks[i].SetActive(i == 2);
                    DifficultyContent[i].SetActive(i != 2);
                    if (i == 2) {
                        DifficultyButtons[i].GetComponent<LevelLock>().isLocked = true;
                    } else if (i != 2) {
                        DifficultyButtons[i].GetComponent<LevelLock>().isLocked = false;
                    }
                }
                break; 
            }
        } else {
            for (int i = 0; i < DifficultyLocks.Length; i++) {
                DifficultyButtons[i].GetComponent<LevelLock>().isLocked = false;
                DifficultyLocks[i].SetActive(false);
                DifficultyContent[i].SetActive(true);
            }
        }
    }
}
