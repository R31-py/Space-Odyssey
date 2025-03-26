using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    private AbilitySlot[] inventory = new AbilitySlot[3];
    private PlayerValues playerValues;

    void Start()
    {
        playerValues = GetComponent<PlayerValues>();
    }

    public bool HasFreeSlot()
    {
        foreach (var slot in inventory)
        {
            if (slot == null) return true;
        }
        return false;
    }

    public bool Add(AbilityItem abilityItem)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == null)
            {
                inventory[i] = new AbilitySlot();
                inventory[i].abilityItem = abilityItem;

                Debug.Log($"Ability: {abilityItem.abilityID} zu Inventar-Slot {i} hinzugefügt!");
                return true;
            }
        }

        Debug.Log("Inventar ist voll!");
        return false;
    }
}
