using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(UniqueID))]
public class ItemPickup : MonoBehaviour//, IPointerClickHandler
{
    //public float pickupRadius = 1f;
    public InvItemData itemData;

    private BoxCollider2D collider;

    [SerializeField] private itemPickupSaveData itemSaveData;
    private string id;

    private void Awake() {
        SaveLoad.OnLoadGame += loadGame;
        itemSaveData = new itemPickupSaveData(itemData, transform.position, transform.rotation);

        collider = GetComponent<BoxCollider2D>();
        collider.isTrigger = true;
        //collider.edgeRadius = pickupRadius;
    }
    private void Start() {
        id = GetComponent<UniqueID>().ID;
        SaveGameManager.data.activeItems.Add(id, itemSaveData);
    }
    private void loadGame(SaveData data) {
        if (data.collectedItems.Contains(id)) Destroy(this.gameObject);
    }
    private void OnDestroy() {
        if (SaveGameManager.data.activeItems.ContainsKey(id)) SaveGameManager.data.activeItems.Remove(id);
        SaveLoad.OnLoadGame -= loadGame;
    }
    /*private void OnTriggerEnter2D(Collider2D collision) {
        var inventory = collision.transform.GetComponent<PlayerInventoryHolder>();
        if (!inventory) return;
        if (inventory.addToInventory(itemData, 1)) {
            SaveGameManager.data.collectedItems.Add(id);
            Destroy(this.gameObject);
        }
    }*/
    public void OnMouseDown() {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (Vector3.Magnitude(player.transform.position - this.gameObject.transform.position) <= 2) {
            var inventory = player.transform.GetComponent<PlayerInventoryHolder>();
            if (!inventory) return;
            if (inventory.addToInventory(itemData, 1)) {
                SaveGameManager.data.collectedItems.Add(id);
                Destroy(this.gameObject);
            }
        }  
    }
}

[System.Serializable]
public struct itemPickupSaveData
{
    public InvItemData itemData;
    public Vector3 position;
    public Quaternion rotation;

    public itemPickupSaveData(InvItemData data, Vector3 position, Quaternion rotation) {
        this.itemData = data;
        this.position = position;
        this.rotation = rotation;
    }
}
