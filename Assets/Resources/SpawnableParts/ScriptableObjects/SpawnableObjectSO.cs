using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SpawnableObjectSO : ScriptableObject
{
    public GameObject partPrefab;
    public Sprite partSprite;

    [Header("Only for whole objects")]
    public Sprite wholeItemDecorSprite;
}
