using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudGeneratorScript : MonoBehaviour
{
    [SerializeField]
    GameObject[] clouds;

    [SerializeField]
    float spawnInterval;

    [SerializeField]
    GameObject endPoint;

    Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        Prewarm();
        Invoke("AttemptSpawn", spawnInterval);
    }
    
    void SpawnCloud(Vector3 startPos)
    {
        // Get Random Cloud from array of clouds
        int randomIndex = UnityEngine.Random.Range(0, clouds.Length);
        GameObject cloud = Instantiate(clouds[randomIndex]);

        // Randomize start y position of cloud (vertically)
        float startY = UnityEngine.Random.Range(startPos.y - 0.4f, startPos.y + 0.7f);
        cloud.transform.position = new Vector3(startPos.x, startY, startPos.z);

        // Randomize scale(size) of cloud
        float scale = UnityEngine.Random.Range(0.5f, 1f);
        cloud.transform.localScale = new Vector3(scale, scale);

        // Randomize opcaity of cloud
        float opacity = UnityEngine.Random.Range(0.6f, 1f);
        var mat = cloud.GetComponent<Renderer>().material;
        Color oldColor = mat.color;
        Color newColor = new Color(oldColor.r, oldColor.g, oldColor.b, opacity);
        mat.SetColor("_Color", newColor);


        // Randomize speed of cloud
        float speed = UnityEngine.Random.Range(0.2f, 0.5f);
        cloud.GetComponent<CloudScript>().StartFloating(speed, endPoint.transform.position.x);


    }

    void AttemptSpawn()
    {
        
        SpawnCloud(startPos);
        Invoke("AttemptSpawn", spawnInterval);

    }

    // Initial Spawn Clouds 
    void Prewarm() 
    {
        for (int i = 0; i < 7; i++) 
        {
            Vector3 spawnPos = startPos + Vector3.right * (i * 2);
            SpawnCloud(spawnPos);
        }
    }

}
