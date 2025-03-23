using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AbilitySlot : MonoBehaviour
{
    public AbilityItem abilityItem;
    SpriteRenderer spriteRenderer;

    private void Update()
    {
        if (abilityItem)
        {
            spriteRenderer.sprite = abilityItem.icon; 
        }
    }
}