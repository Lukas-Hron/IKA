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
        if (obj == null) return;

        Instantiate(obj, spawnPosition, Quaternion.identity);
    }
}
