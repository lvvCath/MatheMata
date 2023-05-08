using UnityEngine;
using System.Collections;
using System;

public class RealTimeClock : MonoBehaviour
{
    public GameObject secondArm;
    public GameObject minuteArm;
    public GameObject hourArm;

    void Update()
    {
        DateTime currentTime = DateTime.Now;
        float second = (float)currentTime.Second;
        float minute = (float)currentTime.Minute;
        float hour = (float)currentTime.Hour;

        float secondAngle = -360 * (second / 60);
        float minuteAngle = -360 * (minute / 60);
        float hourAngle = -360 * (hour / 12);

        secondArm.transform.localRotation = Quaternion.Euler(0, 0, secondAngle);
        minuteArm.transform.localRotation = Quaternion.Euler(0, 0, minuteAngle);
        hourArm.transform.localRotation = Quaternion.Euler(0, 0, hourAngle);

    }

}