using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SpawnableObjectSO : ScriptableObject
{
    public GameObject partPrefab;
    public Sprite partSprite;

    [Header("NOT FOR NORMAL OBJECTS")]
    public Sprite wholeItemDecorSprite;
}
