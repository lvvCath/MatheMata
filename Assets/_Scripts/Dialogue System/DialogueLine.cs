using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DialogueSystem
{
    public class DialogueLine : DialogueBaseClass
    {
        private TMP_Text textHolder;

        [Header ("Text Options")]
        [SerializeField] private string input;
        [SerializeField] private Color32 textColor;
        [SerializeField] private TMP_FontAsset textFont;

        [Header("Time Parameter")]
        [SerializeField] private float delay;

        private void Awake()
        {
            textHolder = GetComponent<TMP_Text>();

            StartCoroutine(WriteText(input, textHolder, textColor, textFont, delay));
        }
    }

}


