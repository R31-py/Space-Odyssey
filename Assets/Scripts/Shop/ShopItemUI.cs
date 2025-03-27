using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemUI : MonoBehaviour
{
    public AbilityItem abilityItem;
    public Image itemImage;
    public Button buyButton;
    
    public void OnBuyButtonClicked()
    {
        // Find the ShopController in the scene and call BuyAbility with this item's abilityItem
        ShopController shopController = FindObjectOfType<ShopController>();
        if (shopController != null)
        {
            shopController.BuyAbility(abilityItem);
        }
    }
}
