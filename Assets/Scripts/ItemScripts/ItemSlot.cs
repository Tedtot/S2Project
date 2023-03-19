using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class ItemSlot : ISerializationCallbackReceiver
{
    [NonSerialized] protected InventoryItemData itemData;
    [SerializeField] protected int itemID = -1;
    [SerializeField] protected int stackSize;

    public InventoryItemData ItemData => itemData;
    public int StackSize => stackSize;
    public void clearSlot() {
        itemData = null;
        itemID = -1;
        stackSize = -1;
    }
    public void assignItem(InventorySlot invSlot) {
        if (itemData == invSlot.ItemData) addToStack(invSlot.stackSize);
        else {
            itemData = invSlot.itemData;
            itemID = itemData.ID;
            stackSize = 0;
            addToStack(invSlot.stackSize);
        }
    }
    public void assignItem(InventoryItemData data, int amount) {
        if (itemData == data) addToStack(amount);
        else {
            itemData = data;
            itemID = data.ID;
            stackSize = 0;
            addToStack(amount);
        }
    }
    public void addToStack(int amount) {
        stackSize += amount;
    }
    public void removeFromStack(int amount) {
        stackSize -= amount;
    }

    public void OnAfterDeserialize()
    {
        if (itemID == -1) return;
        
        var db = Resources.Load<Database>("Database");
        itemData = db.getItem(itemID);        }

    public void OnBeforeSerialize()
    {
        //Nothing
    }
}
