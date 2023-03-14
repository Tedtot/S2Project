using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class InventorySystem
{
    [SerializeField] private List<InventorySlot> invSlots;

    public List<InventorySlot> InvSlots => invSlots;
    public int InvSize => InvSlots.Count;

    public UnityAction<InventorySlot> onInventorySlotChanged;
    public InventorySystem(int size) {
        invSlots = new List<InventorySlot>(size);

        for (int i = 0; i < size; i++) {
            invSlots.Add(new InventorySlot());
        }
    }

    public bool addToInventory(InvItemData item, int amount) {
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

    public bool containsItem(InvItemData item, out List<InventorySlot> invSlot) {
        invSlot = InvSlots.Where(i => i.ItemData == item).ToList();
        return invSlot == null ? false : true;
    }

    public bool hasFreeSlot(out InventorySlot freeSlot) {
        freeSlot = InvSlots.FirstOrDefault(i => i.ItemData == null);
        return freeSlot == null ? false : true;
    }
}
