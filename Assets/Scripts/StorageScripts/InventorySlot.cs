using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySlot : ISerializationCallbackReceiver
{
    [NonSerialized] private InvItemData itemData;
    [SerializeField] private int itemID = -1;
    [SerializeField] private int stackSize;

    public InvItemData ItemData => itemData;
    public int StackSize => stackSize;

    public InventorySlot(InvItemData source, int amount) {
        itemData = source;
        itemID = itemData.ID;
        stackSize = amount;
    }
    public InventorySlot() {
        clearSlot();
    }
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
    public void updateInventorySlot(InvItemData data, int amount) {
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
    public void addToStack(int amount) {
        stackSize += amount;
    }
    public void removeFromStack(int amount) {
        stackSize -= amount;
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

    public void OnBeforeSerialize() {
        //Nothing
    }

    public void OnAfterDeserialize() {
        if (itemID == -1) return;
        
        var db = Resources.Load<Database>("Database");
        itemData = db.getItem(itemID);    
    }
}
