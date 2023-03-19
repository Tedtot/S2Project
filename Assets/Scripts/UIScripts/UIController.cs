using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private ShopKeeperDisplay shopKeeperDisplay;

    private void Awake() {
        shopKeeperDisplay.gameObject.SetActive(false);
    }
    private void OnEnable() {
        ShopKeeper.OnShopWindowRequested += displayShopWindow;
    }
    private void OnDisable() {
        ShopKeeper.OnShopWindowRequested -= displayShopWindow;
    }

    private void displayShopWindow(ShopSystem shopSystem, PlayerInventoryHolder playerInventory)
    {
        shopKeeperDisplay.gameObject.SetActive(true);
        shopKeeperDisplay.displayShopWindow(shopSystem, playerInventory);
    }
}
