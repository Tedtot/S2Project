using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopKeeperDisplay : MonoBehaviour
{
    [SerializeField] private ShopSlotUI shopSlotPrefab;
    [SerializeField] private ShoppingCartItemUI shoppingCartItemPrefab;
    [SerializeField] private Button buyTab;
    [SerializeField] private Button sellTab;

    [Header("Shopping Cart")]
    [SerializeField] private TextMeshProUGUI basketTotalText;
    [SerializeField] private TextMeshProUGUI playerGoldText;
    [SerializeField] private TextMeshProUGUI shopGoldText;
    [SerializeField] private Button buyButton;
    [SerializeField] private TextMeshProUGUI buyButtonText;

    [Header("Item Preview Section")]
    [SerializeField] private Image itemPreviewSprite;
    [SerializeField] private TextMeshProUGUI itemPreviewName;
    [SerializeField] private TextMeshProUGUI itemPreviewDescription;

    [SerializeField] private GameObject itemListContentPanel;
    [SerializeField] private GameObject shoppingCartContentPanel;

    private int basketTotal;
    private bool isSelling;

    private ShopSystem shopSystem;
    private PlayerInventoryHolder playerInvHolder;

    private Dictionary<InventoryItemData, int> shoppingCart = new Dictionary<InventoryItemData, int>();
    private Dictionary<InventoryItemData, ShoppingCartItemUI> shoppingCartUI = new Dictionary<InventoryItemData, ShoppingCartItemUI>();

    public void displayShopWindow(ShopSystem shopSystem, PlayerInventoryHolder playerInvHolder) {
        this.shopSystem = shopSystem;
        this.playerInvHolder = playerInvHolder;

        refreshDisplay();
    }

    private void refreshDisplay() {
        if (buyButton != null) {
            buyButtonText.text = isSelling ? "Sell Items" : "Buy Items";
            buyButton.onClick.RemoveAllListeners();

            if (isSelling) buyButton.onClick.AddListener(sellItems);
            else buyButton.onClick.AddListener(buyItems);
        }

        clearSlots();
        clearItemPreview();

        basketTotalText.enabled = false;
        buyButton.gameObject.SetActive(false);
        basketTotal = 0;
        playerGoldText.text = $"Player Gold: {playerInvHolder.PrimaryInvSystem.Gold}";
        shopGoldText.text = $"Shop Gold: {shopSystem.AvailableGold}";

        
        if (isSelling) displayPlayerInventory();
        else displayShopInventory();
    }

    private void sellItems() {
        if (shopSystem.AvailableGold < basketTotal) return;
        
        foreach (var kvp in shoppingCart) {
            var price = getModifiedPrice(kvp.Key, kvp.Value, shopSystem.SellMarkup);

            shopSystem.sellItem(kvp.Key, kvp.Value, price);

            playerInvHolder.PrimaryInvSystem.gainGold(price);
            updateGoldText();
            playerInvHolder.PrimaryInvSystem.removeItemsFromInventory(kvp.Key, kvp.Value);
        }

        refreshDisplay();
    }
 
    private void buyItems() {
        if (playerInvHolder.PrimaryInvSystem.Gold < basketTotal) return;
        if (!playerInvHolder.PrimaryInvSystem.checkInventoryRemaining(shoppingCart)) return;

        foreach (var kvp in shoppingCart) {
            shopSystem.buyItem(kvp.Key, kvp.Value);

            for (int i = 0; i < kvp.Value; i++) {
                playerInvHolder.PrimaryInvSystem.addToInventory(kvp.Key, 1);
            }
        }

        shopSystem.gainGold(basketTotal);
        playerInvHolder.PrimaryInvSystem.loseGold(basketTotal);
        updateGoldText();

        refreshDisplay();
    }

    private void clearSlots() {
        shoppingCart = new Dictionary<InventoryItemData, int>();
        shoppingCartUI = new Dictionary<InventoryItemData, ShoppingCartItemUI>();

        foreach (var item in itemListContentPanel.transform.Cast<Transform>()) {
            Destroy(item.gameObject);
        }
        foreach (var item in shoppingCartContentPanel.transform.Cast<Transform>()) {
            Destroy(item.gameObject);
        }
    }

    private void displayShopInventory() {
        foreach (var item in shopSystem.ShopInventory) {
            if (item.ItemData == null) continue;;

            var shopSlot = Instantiate(shopSlotPrefab, itemListContentPanel.transform);
            shopSlot.initiate(item, shopSystem.BuyMarkup);
            
        }
    }

    private void displayPlayerInventory() {
        foreach (var item in playerInvHolder.PrimaryInvSystem.getAllItemsHeld()) {
            var tempSlot = new ShopSlot();
            tempSlot.assignItem(item.Key, item.Value);

            var shopSlot = Instantiate(shopSlotPrefab, itemListContentPanel.transform);
            shopSlot.initiate(tempSlot, shopSystem.SellMarkup);
        }
    }

    public void removeItemFromCart(ShopSlotUI shopSlotUI) {
        var data = shopSlotUI.AssignedSlot.ItemData;
        var price = getModifiedPrice(data, 1, shopSlotUI.Markup);

        if (shoppingCart.ContainsKey(data)) {
            shoppingCart[data]--;
            var newString = $"{data.DisplayName} ({price}G) x{shoppingCart[data]}";
            shoppingCartUI[data].setItemText(data.Icon, newString, (price * shoppingCart[data]).ToString());

            if (shoppingCart[data] <= 0) {
                shoppingCart.Remove(data);
                var tempObj = shoppingCartUI[data].gameObject;
                shoppingCartUI.Remove(data);
                Destroy(tempObj);
            }
        }

        basketTotal -= price;
        basketTotalText.text = $"Total: {basketTotal}G";

        if (basketTotal <= 0 && basketTotalText.IsActive()) {
            basketTotalText.enabled = false;
            buyButton.gameObject.SetActive(false);
            clearItemPreview();
            return;
        }

        checkValidPurchase();
    }

    private void clearItemPreview() {
        itemPreviewSprite.sprite = null;
        itemPreviewSprite.color = Color.clear;
        itemPreviewName.text = "";
        itemPreviewDescription.text = "";
    }

    public void addItemToCart(ShopSlotUI shopSlotUI) {
        var data = shopSlotUI.AssignedSlot.ItemData;

        updateItemPreview(shopSlotUI);

        var price = getModifiedPrice(data, 1, shopSlotUI.Markup);

        if (shoppingCart.ContainsKey(data)) {
            shoppingCart[data]++;
            var newString = $"{data.DisplayName} ({price}G) x{shoppingCart[data]}";
            shoppingCartUI[data].setItemText(data.Icon, newString, (price * shoppingCart[data]).ToString());
        }
        else {
            shoppingCart.Add(data, 1);
            
            var shoppingCartTextObj = Instantiate(shoppingCartItemPrefab, shoppingCartContentPanel.transform);
            var newString = $"{data.DisplayName} ({price}G) x1";
            shoppingCartTextObj.setItemText(data.Icon, newString, price.ToString());
            shoppingCartUI.Add(data, shoppingCartTextObj);
        }

        basketTotal += price;
        basketTotalText.text = $"Total: {basketTotal}G";

        if (basketTotal > 0 && !basketTotalText.IsActive()) {
            basketTotalText.enabled = true;
            buyButton.gameObject.SetActive(true);
        }

        checkValidPurchase();
    }

    private void checkValidPurchase() {
        var goldToCheck = isSelling ? shopSystem.AvailableGold : playerInvHolder.PrimaryInvSystem.Gold;
        basketTotalText.color = basketTotal > goldToCheck ? Color.red : Color.white;

        if (isSelling || playerInvHolder.PrimaryInvSystem.checkInventoryRemaining(shoppingCart)) return;

        basketTotalText.text = "Not enough room in inventory";
        basketTotalText.color = Color.red;
    }

    public static int getModifiedPrice(InventoryItemData data, int amount, float markup) {
        var baseValue = data.Gold * amount;

        return Mathf.FloorToInt(baseValue + baseValue * markup);
    }

    private void updateItemPreview(ShopSlotUI shopSlotUI) {
        var data = shopSlotUI.AssignedSlot.ItemData;

        itemPreviewSprite.sprite = data.Icon;
        itemPreviewSprite.color = Color.white;
        itemPreviewName.text = data.DisplayName;
        itemPreviewDescription.text = data.Description;
    }
    
    public void onBuyTabPressed() {
        isSelling = false;
        refreshDisplay();
    }
        
    public void onSellTabPressed() {
        isSelling = true;
        refreshDisplay();
    }

    private void updateGoldText() {
        GameObject.Find("PlayerGold").GetComponent<TextMeshProUGUI>().text = playerInvHolder.PrimaryInvSystem.Gold.ToString() + " G";
    }
}
