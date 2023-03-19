using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlotUI : MonoBehaviour
{
    [SerializeField] private Image itemSprite;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemCount;
    [SerializeField] private ShopSlot assignedSlot;

    public ShopSlot AssignedSlot => assignedSlot;

    [SerializeField] private Button _addToCart;
    [SerializeField] private Button _removeFromCart;

    private int tempAmount;

    public ShopKeeperDisplay ParentDisplay {get; private set; }
    public float Markup { get; private set; }

    private void Awake() {
        clear();

        _addToCart?.onClick.AddListener(addItemToCart);
        _removeFromCart?.onClick.AddListener(removeItemFromCart);
        ParentDisplay = transform.parent.GetComponentInParent<ShopKeeperDisplay>();
    }

    public void initiate(ShopSlot slot, float markup) {
        assignedSlot = slot;
        Markup = markup;
        tempAmount = slot.StackSize;
        updateUISlot();
    }

    private void updateUISlot() {
        if (assignedSlot.ItemData != null) {
            itemSprite.sprite = assignedSlot.ItemData.Icon;
            itemSprite.color = Color.white;
            itemCount.text = assignedSlot.StackSize.ToString();
            var modifiedPrice = ShopKeeperDisplay.getModifiedPrice(assignedSlot.ItemData, 1, Markup);
            itemName.text = $"{assignedSlot.ItemData.DisplayName} x{modifiedPrice}G";
        }
        else {
            clear();
        }
    }

    private void clear() {
        itemSprite.sprite = null;
        itemSprite.preserveAspect = true;
        itemSprite.color = Color.clear;
        itemName.text = "";
        itemCount.text = "";
    }

    private void addItemToCart()
    {
        if (tempAmount <= 0) return;

        tempAmount--;
        ParentDisplay.addItemToCart(this);
        itemCount.text = tempAmount.ToString();   
    }

    private void removeItemFromCart()
    {
        if (tempAmount == assignedSlot.StackSize) return;

        tempAmount++;
        ParentDisplay.removeItemFromCart(this);
        itemCount.text = tempAmount.ToString();
    }

}
