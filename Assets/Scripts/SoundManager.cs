using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//Tutorial: https://www.youtube.com/watch?v=jEoobucfoL4
//Diese Klasse wird nach dem Singleton-Design-Pattern erstellt
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    
    [SerializeField] private SoundLibrary sfxLibrary;
    
    [SerializeField] private AudioSource sfx2DSource;//Fuer UI-Soundeffekte

    private void Awake()
    {
        // Überprüfen, ob bereits eine Instanz existiert
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("A duplicate SoundManager was found and destroyed.");
            Destroy(gameObject); // Zerstört die aktuelle Instanz
            return; // Verhindert, dass weitere Logik ausgeführt wird
        }

        // Wenn keine Instanz existiert, registrieren und über Szenenwechsel hinweg behalten
        Instance = this;
        DontDestroyOnLoad(gameObject);
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
        Debug.Log("Sound played");
        sfx2DSource.PlayOneShot(sfxLibrary.GetClipFromName(soundName));
    }
    
}
