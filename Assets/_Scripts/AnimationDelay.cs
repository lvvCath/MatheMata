using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationDelay : MonoBehaviour
{
    Animator anim;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    public void IncrementAnimationTimer()
    {
        StartCoroutine(WaitForABit());
    }

    IEnumerator WaitForABit()
    {
        while (true)
        {
            anim.SetBool("blink", true);
            yield return new WaitForSeconds(4);
            anim.SetBool("blink", false);
        }
    }
}
