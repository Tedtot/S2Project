using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(UniqueID))]
public class ShopKeeper : MonoBehaviour, IInteractable
{
    [SerializeField] private ShopItemList shopItems;
    [SerializeField] private ShopSystem shopSystem;

    public static UnityAction<ShopSystem, PlayerInventoryHolder> OnShopWindowRequested;
    public static UnityAction OnShopWindowRemoved;

    private ShopSaveData shopSaveData;
    private string id;

    private void Awake() {
        shopSystem = new ShopSystem(shopItems.Items.Count, shopItems.MaxGold, shopItems.BuyMarkup, shopItems.SellMarkup);

        foreach (var item in shopItems.Items) {
            shopSystem.addToShop(item.ItemData, item.Amount);
            //Debug.Log($"{item.ItemData.DisplayName}:{item.Amount}");
        }

        id = GetComponent<UniqueID>().ID;
        shopSaveData = new ShopSaveData(shopSystem);
    }

    private void Start() {
        if (!SaveGameManager.data.ShopKeeperDictionary.ContainsKey(id)) SaveGameManager.data.ShopKeeperDictionary.Add(id, shopSaveData);
    }

    private void OnEnable() {
        SaveLoad.OnLoadGame += loadInventory;
    }

    private void OnDisable() {
         SaveLoad.OnLoadGame -= loadInventory;
    }

    private void loadInventory(SaveData data) {
        if (!data.ShopKeeperDictionary.TryGetValue(id, out ShopSaveData shopSaveData)) return;

        this.shopSaveData = shopSaveData;
        this.shopSystem = shopSaveData.ShopSystem;
    }
    
    public UnityAction<IInteractable> OnInteractionComplete { get; set; }
    public void interact(Interactor interactor, out bool interactSuccessful)
    {
        var playerInv = interactor.GetComponent<PlayerInventoryHolder>();
        
        if (playerInv != null) {
            OnShopWindowRequested?.Invoke(shopSystem, playerInv);
            interactSuccessful = true;
        }
        else {
            interactSuccessful = false;
            Debug.Log("Player inventory not found");
        }
    }

    public void endInteraction()
    {
        OnShopWindowRemoved?.Invoke();
    }
}

[System.Serializable]
public class ShopSaveData {
    public ShopSystem ShopSystem;

    public ShopSaveData(ShopSystem shopSystem) {
        ShopSystem = shopSystem;
    }
}
