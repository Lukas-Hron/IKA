using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI selectedLevelText;

    string selectedLevelName;

    public void SetSelectedLevel(string levelName)
    {
        selectedLevelText.text = levelName;
        selectedLevelName = levelName;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(selectedLevelName);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }
}
