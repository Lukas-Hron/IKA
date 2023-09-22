using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI selectedLevelText;
    string selectedLevelName;

    public void test()
    {
        Debug.Log("hejsan");
    }
    public void FlipIsActive()
    {
        gameObject.SetActive(!gameObject.activeInHierarchy);
    }

    public void LoadLevel()
    {

        if (selectedLevelName != null)
            SceneManager.LoadScene(selectedLevelName);
    }

    public void RestartLevel()
    {
        selectedLevelName = SceneManager.GetActiveScene().name;
        LoadLevel();
    }

    public void SetSelectedLevel(string levelName)
    {
        selectedLevelText.text = levelName;
        selectedLevelName = levelName;
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
