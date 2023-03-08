using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleLeft : MonoBehaviour
{
    [Header("Collisions / Weights")]
    private GameObject objDetected;
    private float objMass;
    private float totalMass;

    private float OnTriggerEnter2D(Collider2D collision)
    {
        objDetected = collision.gameObject;
        objMass = objDetected.GetComponent<Rigidbody2D>().mass;

        totalMass += objMass;

        Debug.Log("Collision mass: " + totalMass);
        return totalMass;
    }

    public float GetTotalMass()
    {
        return totalMass;
    }
}
