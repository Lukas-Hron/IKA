using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointCounter : MonoBehaviour
{
    public int pointsNeededForLevel;
    public int points;

    public void AddPoints(int pointsToAdd)
    {
        points += pointsToAdd;
    }
}
