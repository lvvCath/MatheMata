using UnityEngine;

public abstract class DurstenfeldShuffleAlgorithm
{
    public GameObject[] DurstenfeldShuffle(GameObject[] gameObjectArr) 
    {
        int last_index = gameObjectArr.Length - 1;
        while (last_index > 0)
        {
            int rand_index = Random.Range(0, last_index);
            GameObject temp = gameObjectArr[last_index];
            gameObjectArr[last_index] = gameObjectArr[rand_index];
            gameObjectArr[rand_index] = temp;
            last_index -= 1;
        }
        return gameObjectArr;
    }
}
