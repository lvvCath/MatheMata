using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleLeft : MonoBehaviour
{
    [SerializeField]
    private GameObject RightScale;
    public GameObject scaleBar;
    private int Mass;
    private GameObject objDetected;
    private float objMass;
    
    private void OnEnable()
    {
        RightScale.GetComponent<TriggerScale>().SetLeftScaleMass(20);
    }

    public float GetObjectMass ()
    {
        return Mass;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Mass = 0;
        objDetected = collision.gameObject;
        objMass = objDetected.GetComponent<Rigidbody2D>().mass;
        Mass = (int)objMass;
        RightScale.GetComponent<TriggerScale>().SetLeftScaleMass(Mass);

    }
}
