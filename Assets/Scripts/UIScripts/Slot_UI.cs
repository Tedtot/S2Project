using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class Slot_UI : MonoBehaviour, /*IPointerClickHandler,*/ IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Image itemSprite;
    [SerializeField] private TextMeshProUGUI itemCount;
    [SerializeField] private InventorySlot assignedSlot;
    private MouseItemData mouseData;
    //private Button button;
    public InventorySlot AssignedSlot => assignedSlot;
    public InventoryDisplay ParentDisplay{ get; private set; }
    private void Awake() {
        clearSlot();

        //button = GetComponent<Button>();
        //button?.onClick.AddListener(onUISlotClick);

        mouseData = GameObject.Find("MouseObject").GetComponent<MouseItemData>();
        ParentDisplay = transform.parent.GetComponent<InventoryDisplay>();

    }
    public void initiate(InventorySlot slot) {
        assignedSlot = slot;
        updateUISlot(slot);
    }
    public void updateUISlot(InventorySlot slot) {
        if (slot.ItemData != null) {
            itemSprite.sprite = slot.ItemData.Icon;
            itemSprite.color = Color.white;

            /*if (slot.StackSize > 1)*/ itemCount.text = slot.StackSize.ToString();
            //else itemCount.text = "";
        } 
        else clearSlot();
    }
    public void updateUISlot() {
        if (assignedSlot != null) updateUISlot(assignedSlot);
    }
    public void clearSlot() {
        assignedSlot?.clearSlot();
        itemSprite.sprite = null;
        itemSprite.color = Color.clear;
        itemCount.text = "";
    }
    /*public void onUISlotClick() {
        ParentDisplay?.slotClicked(this);
    }*/
    /*public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Left)
            ParentDisplay?.slotClicked(this, false);
        else if (eventData.button == PointerEventData.InputButton.Right)
            ParentDisplay?.slotClicked(this, true);
    }*/
    public void OnPointerDown(PointerEventData eventData) {
        if (!mouseData.Active) {
            if (eventData.button == PointerEventData.InputButton.Left)
                ParentDisplay?.slotClicked(this, false);
            else if (eventData.button == PointerEventData.InputButton.Right)
                ParentDisplay?.slotClicked(this, true);
        }
    }
    public void OnPointerUp(PointerEventData eventData) {
        var obj = getGameObjectFromPointer();

        if (obj != null && obj.transform.parent.gameObject.tag.Equals("UISlot")) { //Switch Slot
            ParentDisplay?.slotClicked(obj.transform.parent.gameObject.GetComponent<Slot_UI>(), false);
        }
        else if (obj == null) { //Drop
            //Done in mouse script
        }
        //Error, keep in slot
        else ParentDisplay?.slotClicked(this, false);

    }
    private GameObject getGameObjectFromPointer() {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = Mouse.current.position.ReadValue();
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        if (results.Count > 0) return results[0].gameObject;
        else return null;
    }
}
