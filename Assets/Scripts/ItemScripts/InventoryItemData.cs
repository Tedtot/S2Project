using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

[CreateAssetMenu(menuName = "Inventory Item Data")]
public class InventoryItemData : ScriptableObject
{
    [Header("ItemData")]
    public int ID = -1;
    public string DisplayName;
    public int MaxStackSize;
    public int Gold;
    public Sprite Icon;
    public GameObject ItemPrefab;
    public string Description;
    public InventoryItemData PlaceItem;

    public void useItem(Slot_UI slot) {
        if (!isPointerOverUIObject() && isPointerOverItem()) {
            GameObject.Find("Items").GetComponent<UseItemScript>().getAction(slot);
        }
    }

    public bool isPointerOverUIObject() {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = Mouse.current.position.ReadValue();
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    public bool isPointerOverItem() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        if (hit.collider == null || !hit.collider.gameObject.tag.Equals("NoUse")) {
            return true;
        }
        return false;
    }
}
