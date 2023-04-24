using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceItem : ItemAction
{
    [SerializeField] private GameObject player;

    public override void useItem(Slot_UI slot) {
        InventoryItemData data = slot.AssignedSlot.ItemData;

        bool isObj = detectItem();
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 9.99f);

        if (data.PlaceItem.ItemPrefab != null && isObj && Vector3.Magnitude(player.transform.position - mousePos) <= 3) {
            Debug.Log(slot.AssignedSlot.ItemData.DisplayName + " placed");

            slot.AssignedSlot.removeFromStack(1);
            slot.updateUISlot();

            Vector3 placePos = new Vector3(Mathf.RoundToInt(mousePos.x), Mathf.RoundToInt(mousePos.y), mousePos.z);

            Instantiate(data.PlaceItem.ItemPrefab, placePos + new Vector3(0, 0 , 10.01f), Quaternion.identity);
        }
        else Debug.Log("Not a valid spot to place object");

    }

    public bool detectItem() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
        
        if (hit.collider != null) {
            return false;
        }
        return true;
    }
}
