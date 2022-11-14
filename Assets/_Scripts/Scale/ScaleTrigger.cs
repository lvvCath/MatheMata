using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleTrigger : MonoBehaviour
{
    [Header("Scale")]
    public GameObject scaleBar;
    public GameObject LeftScale;
    public float rotateTime;

    [Header("Question")]
    public GameObject Q4;
    public GameObject Q5;

    private GameObject objDetected;
    private float objMass;

    private float scaleRight_totalMass;
    private float scaleLeft_mass;

    private bool isEqual;

    private void OnEnable()
    {
        // if Q4 is active
        if (Q4.activeSelf)
        {
            scaleRight_totalMass = 0;
            scaleLeft_mass = LeftScale.GetComponent<LeftScale>().GetWatermelonMass();
        }

        if (Q5.activeSelf)
        {
            scaleRight_totalMass = 0;
            scaleLeft_mass = LeftScale.GetComponent<LeftScale>().GetPineappleMass();
        }

        scale();

    }

    public float GetTotalMass()
    {
        return scaleRight_totalMass;
    }

    public bool IsEqual()
    {
        return isEqual;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        objDetected = collision.gameObject;
        objMass = objDetected.GetComponent<Rigidbody2D>().mass;

        scaleRight_totalMass += objMass;

        scale();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        objDetected = collision.gameObject;
        objMass = objDetected.GetComponent<Rigidbody2D>().mass;

        scaleRight_totalMass -= objMass;

        scale();
    }

    void scale()
    {
        if (scaleLeft_mass == scaleRight_totalMass) // 0
        {
            isEqual = true;
            LeanTween.rotateZ(scaleBar, 0, rotateTime);
        }

        if (scaleLeft_mass < scaleRight_totalMass) // -10
        {
            isEqual = false;
            LeanTween.rotateZ(scaleBar, -10, rotateTime);
        }

        if (scaleLeft_mass > scaleRight_totalMass) // 10
        {
            isEqual = false;
            LeanTween.rotateZ(scaleBar, 10, rotateTime);
        }
    }

}
