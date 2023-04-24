using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potato : ItemAction
{
    private HealthManager healthManager;

    private void Start() {
        healthManager = GameObject.Find("Health").GetComponent<HealthManager>();
    }

    public override void useItem(Slot_UI slot) {
        InventoryItemData data = slot.AssignedSlot.ItemData;
        
        Debug.Log(slot.AssignedSlot.ItemData.DisplayName + " used");

        slot.AssignedSlot.removeFromStack(1);
        slot.updateUISlot();

        healthManager.healPlayer(15);
    }

    public override GameObject getItem()
    {
        throw new System.NotImplementedException();
    }
}
