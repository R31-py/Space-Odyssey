using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public AbilitySlot[] inventory = new AbilitySlot[3];
    public PlayerValues playerValues;

    public bool HasFreeSlot()
    {
        foreach (AbilitySlot slot in inventory)
        {
            if (slot.isEmpty) return true;
        }
        return false;
    }

    public bool Add(AbilityItem abilityItem)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i].isEmpty)
            {
                inventory[i].SetAbility(abilityItem);
                playerValues.Inventory[i] = abilityItem;

                Debug.Log($"Ability: {abilityItem.abilityID} zu Inventar-Slot {i} hinzugefügt!");
                return true;
            }
        }

        return false;
        Debug.Log("Inventar ist voll!");
    }
    
    public void Remove(int id)
    {
        inventory[id].UnsetAbility();
        playerValues.Inventory[id] = null;
    }
}
