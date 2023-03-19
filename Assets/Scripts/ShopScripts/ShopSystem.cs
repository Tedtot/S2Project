using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
[System.Serializable]
public class ShopSystem
{
    [SerializeField] private List<ShopSlot> shopInventory;
    [SerializeField]private int availableGold;
    [SerializeField]private float buyMarkup;
    [SerializeField]private float sellMarkup;

    public List<ShopSlot> ShopInventory => shopInventory;
    public int AvailableGold => availableGold;
    public float BuyMarkup => buyMarkup;
    public float SellMarkup => sellMarkup;

    public ShopSystem(int size, int gold, float buyMarkup, float sellMarkup) {
        this.availableGold = gold;
        this.buyMarkup = buyMarkup;
        this.sellMarkup = sellMarkup;

        setShopSize(size);
    }

    private void setShopSize(int size) {
        shopInventory = new List<ShopSlot>(size);
        
        for (int i = 0; i < size; i++) {
            shopInventory.Add(new ShopSlot());
        }
    }

    public void addToShop(InventoryItemData data, int amount) {
        if (containsItem(data, out ShopSlot shopSlot)) {
            shopSlot.addToStack(amount);
        }
        var freeSlot = getFreeSlot();
        freeSlot.assignItem(data, amount);
    }

    private ShopSlot getFreeSlot() {
        var freeSlot = shopInventory.FirstOrDefault(i => i.ItemData == null);
        if (freeSlot == null) {
            freeSlot = new ShopSlot();
            shopInventory.Add(freeSlot);
        }
        return freeSlot;
    }

    public bool containsItem(InventoryItemData item, out ShopSlot shopSlot) {
        shopSlot = shopInventory.Find(i => i.ItemData == item);
        return shopSlot != null;
    }

    public void purchaseItem(InventoryItemData data, int amount) {
        if (!containsItem(data, out ShopSlot slot)) return;

        slot.removeFromStack(amount);
    }

    public void gainGold(int basketTotal) {
        availableGold += basketTotal;
    }
}
