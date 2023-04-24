using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestItem : ItemAction
{
    public override void useItem(Slot_UI slot) {
        InventoryItemData data = slot.AssignedSlot.ItemData;

        var obj = getItem();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 9.99f);

        if (obj != null && Vector3.Magnitude(player.transform.position - mousePos) <= 3) { //Make sure to check if it is wood
            List<InventoryItemData> itemDatas = obj.GetComponent<ItemDrop>().ItemDatas;
            InventoryItemData itemData = itemDatas[Random.Range(0, itemDatas.Count)];
                        
            Destroy(obj);

            Instantiate(itemData.ItemPrefab, obj.transform.position, Quaternion.identity);
        } 
        else Debug.Log("Did not find something to mine");
    }

    public override GameObject getItem() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
        
        if (hit.collider != null && hit.collider.gameObject.tag.Equals("Interactable")) {
            return hit.collider.gameObject;
        }
        return null;
    }
}
