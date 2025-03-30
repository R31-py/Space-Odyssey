using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AbilitySlot : MonoBehaviour
{
    public AbilityItem abilityItem;
    public KeyCode keybind;
    public bool isEmpty = true;
    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();
    }

    public void SetAbility(AbilityItem newAbility)
    {
        abilityItem = newAbility;
        if (abilityItem != null)
        {
            image.sprite= abilityItem.icon;
            isEmpty = false;
        }
        else
        {
            isEmpty = true;
            image.sprite = null;
        }
    }
    
    public void UnsetAbility()
    {
        abilityItem = null;
        isEmpty = true;
        image.sprite = null;
    }
    
}