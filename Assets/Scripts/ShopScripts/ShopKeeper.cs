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

    private void Awake() {
        shopSystem = new ShopSystem(shopItems.Items.Count, shopItems.MaxGold, shopItems.BuyMarkup, shopItems.SellMarkup);

        foreach (var item in shopItems.Items) {
            shopSystem.addToShop(item.ItemData, item.Amount);
            //Debug.Log($"{item.ItemData.DisplayName}:{item.Amount}");
        }
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
        
    }
}
