using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VolumeValue : MonoBehaviour
{
    public TextMeshProUGUI numberText;

    public void SetNumberText(float value)
    {
        int whole_value = (int)(value*100);
        numberText.text = whole_value.ToString() + '%';
    }
}
