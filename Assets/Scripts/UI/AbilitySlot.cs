using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AbilitySlot : MonoBehaviour
{
    public AbilityItem abilityItem;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetAbility(AbilityItem newAbility)
    {
        abilityItem = newAbility;
        if (abilityItem != null)
        {
            spriteRenderer.sprite = abilityItem.icon;
        }
        else
        {
            spriteRenderer.sprite = null;
        }
    }
}