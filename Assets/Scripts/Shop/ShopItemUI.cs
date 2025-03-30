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
        ShopController shopController = FindObjectOfType<ShopController>();
        if (shopController != null)
        {
            shopController.BuyAbility(abilityItem);
        }
    }
}
