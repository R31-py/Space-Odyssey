using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AbilityItem : MonoBehaviour
{
    public int cost;
    public int abilityID;
    public KeyCode keybind;
    public Sprite icon; 
    public int quantity;
    
    public void ActivateAbility(GameObject player)
    {
        
        Debug.Log($"Ability {abilityID} aktiviert für Spieler!");
    }
}
