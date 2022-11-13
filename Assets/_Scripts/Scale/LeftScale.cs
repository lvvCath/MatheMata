using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftScale : MonoBehaviour
{
    public GameObject watermelon, pineapple;

    public float GetWatermelonMass()
    {
        return watermelon.GetComponent<Rigidbody2D>().mass;
    }

    public float GetPineappleMass()
    {
        return pineapple.GetComponent<Rigidbody2D>().mass;
    }


}
