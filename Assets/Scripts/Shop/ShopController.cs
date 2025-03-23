using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    public List<ShopItemUI> shopItems;
    public GameObject player;
    private PlayerValues playerValues;

    private void Start()
    {
        playerValues = player.GetComponent<PlayerValues>();
        //UpdateShopItems();
    }

    /*public void UpdateShopItems()
    {
        foreach (var itemUI in shopItems)
        {
            bool canBuy = playerValues.money >= itemUI.abilityItem.cost && playerValues.HasFreeSlot();
            itemUI.buyButton.interactable = canBuy;
            Color itemColor = itemUI.itemImage.color;
            itemColor.a = canBuy ? 1f : 0.5f;
            itemUI.itemImage.color = itemColor;
        }
    }

    public void BuyAbility(AbilityItem item)
    {
        if (playerValues.money >= item.cost && playerValues.HasFreeSlot())
        {
            playerValues.money -= item.cost;
            playerValues.AddAbility(item);
            Debug.Log($"Gekauft: {item.abilityID}");
        }
        else
        {
            Debug.Log("Nicht genug Geld oder kein freier Platz!");
        }
    }*/
}
