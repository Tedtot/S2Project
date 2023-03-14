using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class InventoryUIController : MonoBehaviour
{
    [FormerlySerializedAs("chestPanel")] public DynamicInventoryDisplay InvPanel;
    public DynamicInventoryDisplay BackpackPanel;

    private void Awake() {
        InvPanel.gameObject.SetActive(false);
        BackpackPanel.gameObject.SetActive(false);
    }
    private void OnEnable() {
        InventoryHolder.OnDynamicInvetoryDisplayRequested += displayInventory;
        PlayerInventoryHolder.OnPlayerInventoryDisplayRequested += displayPlayerInventory;
    }
    private void OnDisable() {
        InventoryHolder.OnDynamicInvetoryDisplayRequested -= displayInventory;
        PlayerInventoryHolder.OnPlayerInventoryDisplayRequested -= displayPlayerInventory;
    }

    void Update() {
        if (InvPanel.gameObject.activeInHierarchy && Keyboard.current.escapeKey.wasPressedThisFrame) InvPanel.gameObject.SetActive(false);
        if (BackpackPanel.gameObject.activeInHierarchy && Keyboard.current.escapeKey.wasPressedThisFrame) BackpackPanel.gameObject.SetActive(false);
    }
    void displayInventory(InventorySystem invToDisplay, int offset) {
        InvPanel.gameObject.SetActive(true);
        InvPanel.refreshDynamicInventory(invToDisplay, offset);
    }
    void displayPlayerInventory(InventorySystem invToDisplay, int offset) {
        BackpackPanel.gameObject.SetActive(true);
        BackpackPanel.refreshDynamicInventory(invToDisplay, offset);
    }
}
