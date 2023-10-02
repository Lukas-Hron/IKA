using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCloseBook : MonoBehaviour
{
    private Scrapbook scrapbook;

    private void Start()
    {
        scrapbook = GetComponentInChildren<Scrapbook>();
    }

    public void AnimationDoneShowPage()
    {
        scrapbook.SetupOpenPage();
    }
}
