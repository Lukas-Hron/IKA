using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroLetterAnimation : MonoBehaviour
{
    RawImage raw;
    Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
        raw = GetComponent<RawImage>();
        anim.enabled = false;
        raw.enabled = false;
    }
    public void StartLetterAnim()
    {
        raw.enabled = true;
        anim.enabled = true;
    }
    public void Delayed()
    {


    }

}
