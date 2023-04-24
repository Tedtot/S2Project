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
    public InventoryItemData ItemData;

    private BoxCollider2D collider;

    [SerializeField] private itemPickupSaveData itemSaveData;
    private string id;

    private QuestGiverDisplay questGiverDisplay;

    private void Awake() {
        SaveLoad.OnLoadGame += loadGame;
        itemSaveData = new itemPickupSaveData(ItemData, transform.position, transform.rotation);

        collider = GetComponent<BoxCollider2D>();
        collider.isTrigger = true;
        //collider.edgeRadius = pickupRadius;
    }
    private void Start() {
        id = GetComponent<UniqueID>().ID;
        SaveGameManager.data.ActiveItems.Add(id, itemSaveData);

        questGiverDisplay = GameObject.Find("QuestController").GetComponent<QuestGiverDisplay>();
    }
    private void loadGame(SaveData data) {
        if (data.CollectedItems.Contains(id)) Destroy(this.gameObject);
    }
    private void OnDestroy() {
        if (id != null && SaveGameManager.data.ActiveItems.ContainsKey(id)) SaveGameManager.data.ActiveItems.Remove(id);
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
            if (inventory.addToInventory(ItemData, 1)) {
                SaveGameManager.data.CollectedItems.Add(id);
                Destroy(this.gameObject);

                Quest questItems = questGiverDisplay.getQuestItems();
                if (questGiverDisplay.ActiveQuest && questItems != null) {
                    for (int i = 0; i < questItems.ItemData.Count; i++) {
                        if (ItemData.DisplayName.Equals(questItems.ItemData[i].DisplayName)) {
                            questGiverDisplay.updateQuestValue(i);
                        }
                    }
                }
            }
        }  
    }
}

[System.Serializable]
public struct itemPickupSaveData
{
    public InventoryItemData itemData;
    public Vector3 position;
    public Quaternion rotation;

    public itemPickupSaveData(InventoryItemData data, Vector3 position, Quaternion rotation) {
        this.itemData = data;
        this.position = position;
        this.rotation = rotation;
    }
}
