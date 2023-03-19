using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class TriggerScale : MonoBehaviour
{
    [Header("Scale")]
    public GameObject scaleBar;
    public float rotateTime;

    [Header("Question")]
    private GameObject objDetected;
    private float objMass;

    private int scaleRight_Mass;
    private int scaleLeft_mass;

    private bool isEqual;

    
    private void Awake()
    {
        scaleRight_Mass = 0;
        LeanTween.rotateZ(scaleBar, 10, rotateTime);
    }

    public void SetLeftScaleMass (int LeftMass) 
    {
        scaleLeft_mass = LeftMass;
    }

    public float GetTotalMass()
    {
        return scaleRight_Mass;
    }

    public bool IsEqual()
    {
        return isEqual;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        objDetected = collision.gameObject;
        objMass = objDetected.GetComponent<Rigidbody2D>().mass;
        scaleRight_Mass += (int)objMass;
        scale();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        objDetected = collision.gameObject;
        objMass = objDetected.GetComponent<Rigidbody2D>().mass;
        scaleRight_Mass -= (int)objMass;
        scale();
    }

    void scale()
    {
        if (scaleLeft_mass == scaleRight_Mass) // 0
        {
            isEqual = true;
            LeanTween.rotateZ(scaleBar, 0, rotateTime);
        }

        if (scaleLeft_mass < scaleRight_Mass) // -10
        {
            isEqual = false;
            LeanTween.rotateZ(scaleBar, -10, rotateTime);
        }

        if (scaleLeft_mass > scaleRight_Mass) // 10
        {
            isEqual = false;
            LeanTween.rotateZ(scaleBar, 10, rotateTime);
        }
    }
}
