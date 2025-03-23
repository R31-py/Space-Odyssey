using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    AbilitySlot[] inventory = new AbilitySlot[3];
    private PlayerValues playerValues;
    
    
    // Start is called before the first frame update
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool add(AbilityItem abilityItem)
    {
        for(int i=0; i<inventory.Length; i++)
        {
            if (!inventory[i])
            {
                playerValues.Inventory[i] = abilityItem.abilityID;
                inventory[i].abilityItem = abilityItem;
                return true;
            }
        }

        return false;
    }
}
