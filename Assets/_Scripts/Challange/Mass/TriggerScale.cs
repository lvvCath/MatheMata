using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerScale : MonoBehaviour
{
    [Header("Scale")]
    public GameObject scaleBar;
    public GameObject LeftScale;
    public float rotateTime;

    [Header("Question")]
    private GameObject objDetected;
    private float objMass;

    private float scaleRight_totalMass = 0;
    private float scaleLeft_mass;

    private bool isEqual;
    
    private void OnEnable()
    {
        LeanTween.rotateZ(scaleBar, 10, rotateTime);
        scaleLeft_mass = LeftScale.GetComponent<ScaleLeft>().GetObjectMass();
        scaleRight_totalMass = 0;
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
        //Debug.Log(IsEqual());
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        objDetected = collision.gameObject;
        objMass = objDetected.GetComponent<Rigidbody2D>().mass;

        scaleRight_totalMass -= objMass;

        scale();
        //Debug.Log(IsEqual());
    }

    void scale()
    {
        if (scaleLeft_mass == scaleRight_totalMass) // 0
        {
            isEqual = true;
            Debug.Log(isEqual);
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
