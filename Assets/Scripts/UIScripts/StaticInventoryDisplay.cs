using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticInventoryDisplay : InventoryDisplay
{
    [SerializeField] private InventoryHolder invHolder;
    [SerializeField] protected Slot_UI[] slots;

    protected virtual void OnEnable() {
        PlayerInventoryHolder.OnPlayerInventoryChanged += refreshStaticDisplay;
    }

    protected virtual void OnDisable() {
        PlayerInventoryHolder.OnPlayerInventoryChanged -= refreshStaticDisplay;
    }

    private void refreshStaticDisplay() {

        if (invHolder != null) {
            invSystem = invHolder.PrimaryInvSystem;
            invSystem.onInventorySlotChanged += updateSlot;
        } 
        else Debug.LogWarning($"No inventory assigned to {this.gameObject}");

        assignSlot(invSystem, 0);
    }

    protected override void Start() {
        refreshStaticDisplay();
    }

    public override void assignSlot(InventorySystem invToDisplay, int offset) {
        slotDictionary = new Dictionary<Slot_UI, InventorySlot>();

        for (int i = 0; i < invHolder.Offset; i++) {
            slotDictionary.Add(slots[i], invSystem.InvSlots[i]);
            slots[i].initiate(invSystem.InvSlots[i]);
        }
    }
}
