using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    public List<ShopItemUI> shopItems;
    public GameObject player;
    public InventoryController inventoryController;
    private PlayerValues playerValues;
    public TextMeshProUGUI moneyText;
    private SceneController sceneController;

    private void Start()
    {
        sceneController = FindObjectOfType<SceneController>();
        if (player == null)
        {
            if (sceneController != null)
            {
                player = sceneController.player.gameObject;
            }
        }
        playerValues = player.GetComponent<PlayerValues>();
        inventoryController = inventoryController.GetComponent<InventoryController>();
    }

    

    private void Update()
    {
        UpdateShopItems();
    }

    public void UpdateShopItems()
    {
        foreach (var itemUI in shopItems)
        {
            bool hasSpace = inventoryController.HasFreeSlot(); 
            bool canBuy = playerValues.money >= itemUI.abilityItem.cost && hasSpace;
            
            itemUI.buyButton.interactable = canBuy;
            Color itemColor = itemUI.itemImage.color;
            itemColor.a = canBuy ? 1f : 0.5f;
            itemUI.itemImage.color = itemColor;
        }
    }

    public void BuyAbility(AbilityItem item)
    {
        if (playerValues.money >= item.cost && item.abilityID == 5)
        {
            playerValues.health += 1;

            if (playerValues.health > playerValues.maxHealth)
            {
                playerValues.maxHealth += 1;
            }
            
            playerValues.money -= item.cost;
        }
        
        else if (playerValues.money >= item.cost && inventoryController.HasFreeSlot()) 
        {
            playerValues.money -= item.cost;
            Debug.Log($"Gekauft: {item.abilityID}");
            inventoryController.Add(item);
            
            UpdateMoney();
        }
        else
        {
            Debug.Log("Nicht genug Geld oder kein freier Platz!");
        }
    }

    public void UpdateMoney()
    {
        moneyText.text = playerValues.money.ToString();
    }
    
}