using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] Transform objectSpawnPosition;
    private Vector3 spawnPosition;

    private bool canSpawn = false;

    private void Awake() => spawnPosition = objectSpawnPosition.position;

    public void SpawnObject(GameObject obj)
    {
        if (obj == null || !canSpawn) return;

        Instantiate(obj, spawnPosition, Quaternion.identity);

        canSpawn = false;
        Invoke(nameof(SpawnCoolDown), 0.5f);
    }

    private void SpawnCoolDown() => canSpawn = true;
}
