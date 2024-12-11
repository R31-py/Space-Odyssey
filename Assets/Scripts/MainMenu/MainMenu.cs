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
    
    public void StartGame()
    {
        LoadVolume();
        SceneManager.LoadScene(1);
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
