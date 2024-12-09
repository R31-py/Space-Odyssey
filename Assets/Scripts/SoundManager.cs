using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Tutorial: https://www.youtube.com/watch?v=jEoobucfoL4
//Diese Klasse wird nach dem Singleton-Design-Pattern erstellt
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    
    [SerializeField] private SoundLibrary sfxLibrary;
    
    [SerializeField] private AudioSource sfx2DSource;//Fuer UI-Soundeffekte

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);//stellt sicher, dass es waehrend Szenen-Wandlungen besteht
        }
        
    }

    //Spielt den Soundeffekt wo man es gesetzt hat, d.h. wenn das Objekt weit weg ist wird es nicht gut gehoert
    public void PlaySound3D(AudioClip clip, Vector3 position)
    {
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, position);
        }
        
    }
    public void PlaySound3D(string soundName, Vector3 position)
    {
        PlaySound3D(sfxLibrary.GetClipFromName(soundName), position);
    }

    public void PlaySound2D(string soundName)
    {
        sfx2DSource.PlayOneShot(sfxLibrary.GetClipFromName(soundName));
    }
    
}
