using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static Action TestingAttentionPlease;

    int amountOfColliders = 0;
    int amountOfOverlappingColliders = 0;
    float negativePoints = 0;

    private void OnEnable()
    {
        TestPointCheck.sendScoreInfo += AddingToScore;
    }
    private void OnDisable()
    {
        TestPointCheck.sendScoreInfo -= AddingToScore;
    }
    private void AddingToScore(int amountOverlap, int amountColliders, bool rightMesh)
    {
        if(rightMesh)
        {
            amountOfOverlappingColliders += amountOverlap;
            amountOfColliders += amountColliders;
            return;
        }

        amountOfOverlappingColliders += 1; //punishment for being wrong
        amountOfColliders += 10;
    }
    [ContextMenu("GetScore")]
    public float GetScore()
    {
        float score = (float)amountOfOverlappingColliders / (float)amountOfColliders;

        Debug.Log(score);
        Debug.Log(amountOfColliders);
        return score;
    }

    [ContextMenu ("Test")]
    public void Test()
    {
        TestingAttentionPlease.Invoke();
    }
}
