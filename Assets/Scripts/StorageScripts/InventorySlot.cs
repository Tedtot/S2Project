using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySlot : ItemSlot
{
    public InventorySlot(InventoryItemData source, int amount) {
        itemData = source;
        itemID = itemData.ID;
        stackSize = amount;
    }
    public InventorySlot() {
        clearSlot();
    }
    public void updateInventorySlot(InventoryItemData data, int amount) {
        itemData = data;
        itemID = data.ID;
        stackSize = amount;
    }
    public bool roomInStack(int amountToAdd, out int amountRemaining) {
        amountRemaining = ItemData.MaxStackSize - stackSize;
        return roomInStack(amountToAdd);
    }
    public bool roomInStack(int amountToAdd) {
        if (stackSize + amountToAdd <= itemData.MaxStackSize) return true;
        else return false;
    }
    public bool splitStack(out InventorySlot splitStack) {
        if (stackSize <= 1) {
            splitStack = null;
            return false;
        }

        int halfStack = Mathf.RoundToInt(stackSize / 2);
        removeFromStack(halfStack);

        splitStack = new InventorySlot(itemData, halfStack);
        return true;
    }
}
