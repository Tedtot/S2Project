using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShoppingCartItemUI : MonoBehaviour
{
    [SerializeField] private Image itemSprite;
    [SerializeField] private TextMeshProUGUI itemText;
    [SerializeField] private TextMeshProUGUI totalCostText;

    public void setItemText(Sprite itemSprite, string itemText, string totalCostText) {
        this.itemSprite.sprite = itemSprite;
        this.itemText.text = itemText;
        this.totalCostText.text = totalCostText + "G";
    }
}
