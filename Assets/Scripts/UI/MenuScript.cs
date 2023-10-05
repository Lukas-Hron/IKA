using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class MenuScript : MonoBehaviour
{
    public event Action AddMusicVolume;
    public event Action RemoveMusicVolume;
    public event Action RemoveSFXVolume;
    public event Action AddSFXVolume;

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

    public void AddVolumeMusic() => AddMusicVolume?.Invoke();
    public void RemoveVolumeMusic() => RemoveMusicVolume?.Invoke();

    public void AddVolumeSfx() => AddSFXVolume?.Invoke();
    public void RemoveVolumeSfx() => RemoveSFXVolume?.Invoke();

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }
}
