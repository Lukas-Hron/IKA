using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    string selectedLevelName;

    public void FlipIsActive()
    {
        gameObject.SetActive(!gameObject.activeInHierarchy);
    }

    public void RestartLevel()
    {
        selectedLevelName = SceneManager.GetActiveScene().name;
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
