using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class CreateSpawnableObjectSOEditor : Editor
{
    [MenuItem("Assets/Create SpawnableObjectSO from this")]
    static void CreateSOFromObject()
    {
        //Get selected object in scene
        Object selectedPrefab = Selection.activeObject;

        if (selectedPrefab != null && selectedPrefab is GameObject)
        {
            GameObject prefab = (GameObject)selectedPrefab;

            //Setup new SpawnableObjectSO
            SpawnableObjectSO spawnableSO = ScriptableObject.CreateInstance<SpawnableObjectSO>();

            spawnableSO.name = prefab.name;
            spawnableSO.partPrefab = prefab;

            //Create asset at path
            string assetPath = "Assets/Resources/SpawnableParts/ScriptableObjects/" + selectedPrefab.name + ".asset";
            AssetDatabase.CreateAsset(spawnableSO, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Selection.activeObject = spawnableSO;
        }
    }
}
