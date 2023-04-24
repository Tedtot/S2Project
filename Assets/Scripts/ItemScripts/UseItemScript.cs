using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseItemScript : MonoBehaviour
{
    [SerializeField] private Dictionary<string, ItemAction> objectActions;    
    [SerializeField] private List<ItemAction> items;

    private void Start() {
        objectActions = new Dictionary<string, ItemAction>();
        foreach (var item in items) objectActions.Add(item.Key, item);
    }

    public void getAction(Slot_UI slot) {
        InventoryItemData data = slot.AssignedSlot.ItemData;

        if (objectActions.ContainsKey(data.DisplayName)) {
            var action = GameObject.Find(data.DisplayName).GetComponent<ItemAction>();
            if (action != null) action.useItem(slot);
            else Debug.Log("Script not found");
        }

        else Debug.Log("Action not found");
    }
}
