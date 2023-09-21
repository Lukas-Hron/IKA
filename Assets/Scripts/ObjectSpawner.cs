using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] Transform objectSpawnPosition;
    private static Vector3 spawnPosition;

    private void Awake() => spawnPosition = objectSpawnPosition.position;

    public static void SpawnObject(GameObject obj)
    {
        Debug.Log("hej");
        if (obj == null) return;
        Debug.Log("tjotjo");

        Instantiate(obj, spawnPosition, Quaternion.identity);
    }
}
