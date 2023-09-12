using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointChecker : MonoBehaviour
{
    public GameObject otherToCheck;
    private PointCounter pointCounter;


    private void Start()
    {
        pointCounter = FindObjectOfType<PointCounter>();

        CheckPoints();
    }

    public void CheckPoints()
    {
        int positionPoints = CheckPosition();
        int rotationPoints = CheckRotation();

        pointCounter.AddPoints(positionPoints + rotationPoints);
    }

    private int CheckPosition()
    {
        int points = 0;

        Vector3 posDiff = otherToCheck.transform.position - transform.position;

        float posThreshold1 = 0.15f;//more points the closer you are
        float posThreshold2 = 0.1f;
        float posThreshold3 = 0.05f;

        if (posDiff.magnitude <= posThreshold3)
        {
            points = 3;
        }
        else if (posDiff.magnitude <= posThreshold2)
        {
            points = 2;
        }
        else if (posDiff.magnitude <= posThreshold1)
        {
            points = 1;
        }

        return points;
    }

    private int CheckRotation()
    {
        int points = 0;

        Quaternion rotDiff = Quaternion.Inverse(otherToCheck.transform.rotation) * transform.rotation;

        float xAngleDiff = Mathf.Abs(rotDiff.x);
        float yAngleDiff = Mathf.Abs(rotDiff.y);
        float zAngleDiff = Mathf.Abs(rotDiff.z);

        float rotThreshold1 = 0.15f;//isch 17 graders gräns
        float rotThreshold2 = 0.1f; //isch 11 graders gräns
        float rotThreshold3 = 0.05f;//isch 5 graders gräns

        if (xAngleDiff <= rotThreshold3 && yAngleDiff <= rotThreshold3 && zAngleDiff <= rotThreshold3)
        {
            points = 3;
        }
        else if (xAngleDiff <= rotThreshold2 && yAngleDiff <= rotThreshold2 && zAngleDiff <= rotThreshold2)
        {
            points = 2;
        }
        else if (xAngleDiff <= rotThreshold1 && yAngleDiff <= rotThreshold1 && zAngleDiff <= rotThreshold1)
        {
            points = 1;
        }

        return points;
    }
}
