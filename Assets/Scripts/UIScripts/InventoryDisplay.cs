using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public abstract class InventoryDisplay : MonoBehaviour
{
    [SerializeField] MouseItemData mouseInvItem;
    protected InventorySystem invSystem;
    protected Dictionary<Slot_UI, InventorySlot> slotDictionary;
    public InventorySystem InvSystem => invSystem;
    public Dictionary<Slot_UI, InventorySlot> SlotDictionary => slotDictionary;

    protected virtual void Start() {

    }

    public abstract void assignSlot(InventorySystem invToDisplay, int offset);

    protected virtual void updateSlot(InventorySlot updatedSlot) {
        foreach (var slot in slotDictionary) {
            if (slot.Value == updatedSlot) { //Slot value - Actual memory
                slot.Key.updateUISlot(updatedSlot); //Slot key - UI
            }
        }
    }

    public void slotClicked(Slot_UI clickedSlot, bool isSplit) {
        //Clicked slot has an item - mouse does not have an item
        if (clickedSlot.AssignedSlot.ItemData != null && mouseInvItem.AssignedSlot.ItemData == null) {
            //Split stack
            if (isSplit && clickedSlot.AssignedSlot.splitStack(out InventorySlot halfStackSlot)) {
                mouseInvItem.updateMouseSlot(halfStackSlot);
                clickedSlot.updateUISlot();
                return;
            }
            else {
                mouseInvItem.updateMouseSlot(clickedSlot.AssignedSlot);
                clickedSlot.clearSlot();
                return;
            }
        }

        //Clicked slot does not have an item - mouse has an item
        if (clickedSlot.AssignedSlot.ItemData == null && mouseInvItem.AssignedSlot.ItemData != null) {
            clickedSlot.AssignedSlot.assignItem(mouseInvItem.AssignedSlot);
            clickedSlot.updateUISlot();

            mouseInvItem.clearSlot();
            return;
        }

        //Both slots have items
        if (clickedSlot.AssignedSlot.ItemData != null && mouseInvItem.AssignedSlot.ItemData != null) {
            //Items same
            bool isSameItem = clickedSlot.AssignedSlot.ItemData == mouseInvItem.AssignedSlot.ItemData;

            if (isSameItem && clickedSlot.AssignedSlot.roomInStack(mouseInvItem.AssignedSlot.StackSize)) { //Room left in stack
                clickedSlot.AssignedSlot.assignItem(mouseInvItem.AssignedSlot);
                clickedSlot.updateUISlot();

                mouseInvItem.clearSlot();
                return;
            }
            else if (isSameItem && !clickedSlot.AssignedSlot.roomInStack(mouseInvItem.AssignedSlot.StackSize, out int leftInStack)) { //No more room in stack
                if (leftInStack < 1) { //Stack is full, swap
                    swapSlots(clickedSlot);
                    return;
                }
                else { //Slot not full, fill stack
                    int remainingOnMouse = mouseInvItem.AssignedSlot.StackSize - leftInStack;

                    clickedSlot.AssignedSlot.addToStack(leftInStack);
                    clickedSlot.updateUISlot();

                    var newItem = new InventorySlot(mouseInvItem.AssignedSlot.ItemData, remainingOnMouse);
                    mouseInvItem.clearSlot();
                    mouseInvItem.updateMouseSlot(newItem);
                    return;
                }
            }
            //Items different
            else if (!isSameItem) { 
                swapSlots(clickedSlot);
                return;
            }

            /*Items differents
            if (clickedSlot.AssignedSlot.ItemData != mouseInvItem.AssignedSlot.ItemData) {
                swapSlots(clickedSlot);
            }*/
        }
    }
    private void swapSlots(Slot_UI clickedSlot) {
        var clonedSlot = new InventorySlot(mouseInvItem.AssignedSlot.ItemData, mouseInvItem.AssignedSlot.StackSize);
        mouseInvItem.clearSlot();

        mouseInvItem.updateMouseSlot(clickedSlot.AssignedSlot);

        clickedSlot.clearSlot();
        clickedSlot.AssignedSlot.assignItem(clonedSlot);
        clickedSlot.updateUISlot();
    }
}

