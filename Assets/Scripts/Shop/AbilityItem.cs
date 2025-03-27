using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AbilityItem 
{
    public int cost;
    public int abilityID;
    public Sprite icon; 
    public int quantity;
    public float cooldown;
    
    public void ActivateAbility(GameObject player)
    {
        
        Debug.Log($"Ability {abilityID} aktiviert für Spieler!");
    }
}
