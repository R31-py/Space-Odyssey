using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Tutorial: https://www.youtube.com/watch?v=jEoobucfoL4
//struct = Structure ist funktionsweise aehnlich wie eine Klasse
//https://www.geeksforgeeks.org/c-sharp-structures-set-1/
[System.Serializable]
public struct SoundEffect
{
    public string groupID;
    public AudioClip[] clips;
}
public class SoundLibrary : MonoBehaviour
{
    public SoundEffect[] soundEffects;

    public AudioClip GetClipFromName(string name)
    {
        //Diese Funktion geht durch alle Soundeffekte
        foreach (var soundEffect in soundEffects)
        {
            //Falls die Name entspricht den groupID dann wird ein zufaelliges Clip von dieser Soundeffekt-Gruppe zurueckgegeben
            if (soundEffect.groupID == name)
            {
                return soundEffect.clips[Random.Range(0, soundEffect.clips.Length)];
            }
        }
        return null;
    }
}
