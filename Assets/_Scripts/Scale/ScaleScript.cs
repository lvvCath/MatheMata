using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleScript : MonoBehaviour
{
    public GameObject scaleLeft, scaleRight;

    public float rotateTime;

    private float scaleLeft_mass, scaleRight_mass;

    // Start is called before the first frame update
    void Start()
    {
        //scaleLeft_mass = scaleLeft.GetComponent<LeftScale>().GetMass();
        scaleRight_mass = scaleRight.GetComponent<ScaleTrigger>().GetTotalMass();
    }

    // Update is called once per frame
    void Update()
    {
        //scaleLeft_mass = scaleLeft.GetComponent<LeftScale>().GetMass();
        scaleRight_mass = scaleRight.GetComponent<ScaleTrigger>().GetTotalMass();

        if (scaleLeft_mass == scaleRight_mass) // 0
        {
            LeanTween.rotateZ(gameObject, 0, rotateTime);
        }

        if (scaleLeft_mass < scaleRight_mass) // -15
        {
            LeanTween.rotateZ(gameObject, -10, rotateTime);
        }

        if (scaleLeft_mass > scaleRight_mass) // 15
        {
            LeanTween.rotateZ(gameObject, 10, rotateTime);
        }

        
    }
}
