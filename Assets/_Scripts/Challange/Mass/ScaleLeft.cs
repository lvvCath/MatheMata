using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleLeft : MonoBehaviour
{
    public float Mass;
    private GameObject objDetected;
    private float objMass;
    
    public void SetHeavyObject(GameObject currObject)
    {
        HeavyObject = currObject;

    }

    public float GetObjectMass ()
    {
        return HeavyObject.GetComponent<Rigidbody2D>().mass;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        objDetected = collision.gameObject;
        objMass = objDetected.GetComponent<Rigidbody2D>().mass;

        Mass += objMass;
    }
}
