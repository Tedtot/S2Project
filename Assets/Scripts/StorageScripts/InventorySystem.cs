using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

[System.Serializable]
public class InventorySystem
{
    [SerializeField] private List<InventorySlot> invSlots;
    [SerializeField] private int gold;

    public int Gold => gold;

    public List<InventorySlot> InvSlots => invSlots;
    public int InvSize => InvSlots.Count;

    public UnityAction<InventorySlot> onInventorySlotChanged;

    public InventorySystem(int size) {
        gold = 0;
        createInventory(size);
    }

    public InventorySystem(int size, int gold) {
        this.gold = gold;
        createInventory(size);
    }

    private void createInventory(int size) {
        invSlots = new List<InventorySlot>(size);

        for (int i = 0; i < size; i++) {
            invSlots.Add(new InventorySlot());
        }
    }

    public bool addToInventory(InventoryItemData item, int amount) {
        if (containsItem(item, out List<InventorySlot> invSlot)) { //Check whether item exists in inventory
            foreach (var slot in invSlot) {
                if (slot.roomInStack(amount)) {
                    slot.addToStack(amount);
                    onInventorySlotChanged?.Invoke(slot);
                    return true;
                }
            }
        }
        if (hasFreeSlot(out InventorySlot freeSlot)) { //Gets first available slot
            freeSlot.updateInventorySlot(item, amount);
            onInventorySlotChanged?.Invoke(freeSlot);
            return true;
        }
        return false;
    }

    public bool containsItem(InventoryItemData item, out List<InventorySlot> invSlot) {
        invSlot = InvSlots.Where(i => i.ItemData == item).ToList();
        return invSlot == null ? false : true;
    }

    public bool hasFreeSlot(out InventorySlot freeSlot) {
        freeSlot = InvSlots.FirstOrDefault(i => i.ItemData == null);
        return freeSlot == null ? false : true;
    }

    public bool checkInventoryRemaining(Dictionary<InventoryItemData, int> shoppingCart) {
        var clonedSystem = new InventorySystem(this.InvSize);

        for (int i = 0; i < InvSize; i++) {
            clonedSystem.InvSlots[i].assignItem(this.InvSlots[i].ItemData, this.InvSlots[i].StackSize);
        }

        foreach (var kvp in shoppingCart) {
            for (int i = 0; i < kvp.Value; i++) {
                if (!clonedSystem.addToInventory(kvp.Key, 1)) return false;
            }
        }
        
        return true;
    }



    public Dictionary<InventoryItemData, int> getAllItemsHeld() {
        var distinctItems = new Dictionary<InventoryItemData, int>();

        foreach (var item in invSlots) {
            if (item.ItemData == null) continue;

            if (!distinctItems.ContainsKey(item.ItemData)) distinctItems.Add(item.ItemData, item.StackSize);
            else distinctItems[item.ItemData] += item.StackSize;
        }
        
        return distinctItems;
    }

    public void loseGold(int amount) {
        gold -= amount;
    }

    public void gainGold(int amount) {
        gold += amount;
    }

    public void removeItemsFromInventory(InventoryItemData data, int amount) {
        if (containsItem(data, out List<InventorySlot> invSlot)) {
            foreach (var slot in invSlot) {
                var stackSize = slot.StackSize;
                
                if (stackSize > amount) slot.removeFromStack(amount);
                else {
                    slot.removeFromStack(stackSize);
                    amount -= stackSize;
                }

                onInventorySlotChanged?.Invoke(slot);
            }
        }
    }
}
