using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Shop System/Shop Item List")]
public class ShopItemList : ScriptableObject
{
    [SerializeField] private List<ShopInvItem> items;
    [SerializeField] private int maxGold;
    [SerializeField] private float sellMarkup;
    [SerializeField] private float buyMarkup;

    public List<ShopInvItem> Items => items;
    public int MaxGold => maxGold;
    public float SellMarkup => sellMarkup;
    public float BuyMarkup => buyMarkup;
}

[System.Serializable]
public struct ShopInvItem {
    public InventoryItemData ItemData;
    public int Amount;
}
