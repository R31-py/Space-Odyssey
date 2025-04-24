using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
//Tutorial: https://www.youtube.com/watch?v=ivvv8kld6_0
public class MainMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    
    public Slider musicSlider;
    public Slider sfxSlider;
    
    public GameObject loadingScreen;
    public Slider progressBar;

    private void Start()
    {
        MusicManager.Instance.PlayMusic("Main Menu");
    }
    
    public void OnStartButtonClicked()
    {
        StartCoroutine(StartGame());
    }


    public IEnumerator StartGame()
    {
        PlayerSaveManager.SaveLoadState.loadingFromSave = true;
        LoadVolume();

        string lastScene = PlayerPrefs.GetString("LastScene", "Level1");

        if (loadingScreen != null) loadingScreen.SetActive(true);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(lastScene);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);

            if (progressBar != null)
                progressBar.value = progress;

            if (asyncLoad.progress >= 0.9f)
            {
                yield return new WaitForSeconds(0.5f); // for smooth visual transition
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }

        MusicManager.Instance.PlayMusic("background");
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    //Grifft auf die Parameter von AudioMixer, die in UnityEditor gesetzt sind,zu und aktualisiert seinen Float-Wert
    public void UpdateMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume);
    }
    
    public void UpdateSFXVolume(float volume)
    {
        audioMixer.SetFloat("SfxVolume", volume);
    }
    
    //speichert die Daten
    public void SaveVolume()
    {
        audioMixer.GetFloat("MusicVolume", out float musicvolume);
        PlayerPrefs.SetFloat("MusicVolume", musicvolume);
        
        audioMixer.GetFloat("SfxVolume", out float sfxvolume);
        PlayerPrefs.SetFloat("SfxVolume", sfxvolume);
    }
    
    //ladet die gespeicherte Daten in den jeweiligen Sliders
    public void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("SfxVolume");
        
    }
}
