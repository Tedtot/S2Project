using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections.Generic;

public class MouseItemData : MonoBehaviour
{
    public Image ItemSprite;
    public TextMeshProUGUI ItemCount;
    public InventorySlot AssignedSlot;
    public bool Active;
    private Transform playerTransform;

    private void Awake() {
        ItemSprite.color = Color.clear;
        ItemCount.text = "";
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        Active = false;
        if (playerTransform == null) Debug.Log("Player not found.");
    }

    public void updateMouseSlot(InventorySlot invSlot) {
        AssignedSlot.assignItem(invSlot);
        updateMouseSlot();
    }
    public void updateMouseSlot() {
        ItemSprite.sprite = AssignedSlot.ItemData.Icon;
        ItemCount.text = AssignedSlot.StackSize.ToString();
        ItemSprite.color = Color.white;
        Active = true;
    }

    private void Update() {
        if (AssignedSlot.ItemData != null) {
            transform.position = Mouse.current.position.ReadValue();

            if (Mouse.current.leftButton.wasPressedThisFrame && !isPointerOverUIObject()) {
                if (AssignedSlot.ItemData.ItemPrefab != null) {
                    for (int i = 0; i < AssignedSlot.StackSize; i++) {
                        Instantiate(AssignedSlot.ItemData.ItemPrefab, playerTransform.position + new Vector3(Random.Range(-0.75f, 0.75f), Random.Range(-0.75f, 0.75f), 0), Quaternion.identity);
                    }
                    clearSlot();
                } 
                else Debug.Log("Item prefab not found.");
            } 
            else if (Mouse.current.rightButton.wasPressedThisFrame && !isPointerOverUIObject()) {
                if (AssignedSlot.ItemData.ItemPrefab != null) {
                    Instantiate(AssignedSlot.ItemData.ItemPrefab, playerTransform.position + new Vector3(Random.Range(-0.75f, 0.75f), Random.Range(-0.75f, 0.75f), 0), Quaternion.identity);

                    if (AssignedSlot.StackSize > 1) {
                        AssignedSlot.addToStack(-1);
                        updateMouseSlot();
                    }
                    else clearSlot();
                } 
                else Debug.Log("Item prefab not found.");
            }
        }
    }
    public void clearSlot() {
        AssignedSlot.clearSlot();
        ItemCount.text = "";
        ItemSprite.color = Color.clear;
        ItemSprite.sprite = null;
        Active = false;
    }

    public static bool isPointerOverUIObject() {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = Mouse.current.position.ReadValue();
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}
