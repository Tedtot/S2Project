using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(UniqueID))]
public class ChestInventory : InventoryHolder, IInteractable
{
    public UnityAction<IInteractable> OnInteractionComplete{ get; set; }
    protected override void Awake() {
        base.Awake();
        SaveLoad.OnLoadGame += loadInventory;
    }

    private void Start() {
        var chestSaveData = new invSaveData(primaryInvSystem, transform.position, transform.rotation);
        SaveGameManager.data.ChestDictionary.Add(GetComponent<UniqueID>().ID, chestSaveData);
    }

    protected override void loadInventory(SaveData data) {
        //Check save data for specific data and loads it
        if (data.ChestDictionary.TryGetValue(GetComponent<UniqueID>().ID, out invSaveData chestData)) {
            this.primaryInvSystem = chestData.InvSystem;
            this.transform.position = chestData.Position;
            this.transform.rotation = chestData.Rotation;
        }
    }

    public void interact(Interactor interactor, out bool interactSuccessful) {
        OnDynamicInventoryDisplayRequested?.Invoke(primaryInvSystem, 0);
        interactSuccessful = true;
    }

    public void endInteraction() { //Fires when the player exits
        OnDynamicInventoryDisplayRemoved?.Invoke();
    }
}