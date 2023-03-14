using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
public class PlayerInventoryHolder : InventoryHolder
{
    public static UnityAction OnPlayerInventoryChanged;
    public static UnityAction<InventorySystem, int> OnPlayerInventoryDisplayRequested;
    private void Start() {
        SaveGameManager.data.playerInventory = new invSaveData(primaryInvSystem);
    }
    protected override void loadInventory(SaveData data) {
        //Check save data for specific data and loads it
        if (data.playerInventory.InvSystem != null) {
            this.primaryInvSystem = data.playerInventory.InvSystem;
            OnPlayerInventoryChanged?.Invoke();
        }
    }
    void Update() {
        if (Keyboard.current.spaceKey.wasPressedThisFrame) OnPlayerInventoryDisplayRequested?.Invoke(primaryInvSystem, 9);
    }
    public bool addToInventory(InvItemData data, int amount) {
        if (primaryInvSystem.addToInventory(data, amount)) {
            return true;
        }
        return false;        
    }
}
