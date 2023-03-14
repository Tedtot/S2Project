using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DynamicInventoryDisplay : InventoryDisplay
{
    [SerializeField] protected Slot_UI slotPrefab;
    protected override void Start() {
        base.Start();
    }
    public void refreshDynamicInventory(InventorySystem invToDisplay, int offset) {
        clearSlots();
        invSystem = invToDisplay;
        if (invSystem != null) invSystem.onInventorySlotChanged += updateSlot;
        assignSlot(invToDisplay, offset);
    }
    public override void assignSlot(InventorySystem invToDisplay, int offset) {
        slotDictionary = new Dictionary<Slot_UI, InventorySlot>();

        if (invToDisplay == null) return;

        for (int i = offset; i < invToDisplay.InvSize; i++) {
            var uiSlot = Instantiate(slotPrefab, transform);
            slotDictionary.Add(uiSlot, invToDisplay.InvSlots[i]);
            uiSlot.initiate(invToDisplay.InvSlots[i]);
            uiSlot.updateUISlot();
        }
    }
    private void clearSlots() {
        foreach (var item in transform.Cast<Transform>()) {
            Destroy(item.gameObject); //Should use object pooling instead of destroying and instantiating the slots
        }

        if (slotDictionary != null) slotDictionary.Clear();
    }
    private void OnDisable() {
        if (invSystem != null) invSystem.onInventorySlotChanged -= updateSlot;
    }
}
