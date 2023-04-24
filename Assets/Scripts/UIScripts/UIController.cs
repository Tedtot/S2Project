using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIController : MonoBehaviour
{
    [SerializeField] private ShopKeeperDisplay shopKeeperDisplay;
    [SerializeField] private QuestGiverDisplay questGiverDisplay;
    [SerializeField] private GameObject questGiverDisplayMain;

    private void Awake() {
        shopKeeperDisplay.gameObject.SetActive(false);
        questGiverDisplayMain.SetActive(false);
    }

    private void OnEnable() {
        ShopKeeper.OnShopWindowRequested += displayShopWindow;
        ShopKeeper.OnShopWindowRemoved += removeWindow;

        QuestGiver.OnQuestWindowRequested += displayQuestWindow;
        QuestGiver.OnQuestWindowRemoved += removeWindow;

    }

    private void OnDisable() {
        ShopKeeper.OnShopWindowRequested -= displayShopWindow;
        ShopKeeper.OnShopWindowRemoved -= removeWindow;

        QuestGiver.OnQuestWindowRequested -= displayQuestWindow;
        QuestGiver.OnQuestWindowRemoved -= removeWindow;

    }

    private void Update() {
        if (Keyboard.current.escapeKey.wasPressedThisFrame) {
            shopKeeperDisplay.gameObject.SetActive(false);
            questGiverDisplayMain.SetActive(false);
        }
    }

    private void displayShopWindow(ShopSystem shopSystem, PlayerInventoryHolder playerInventory) {
        shopKeeperDisplay.gameObject.SetActive(true);
        shopKeeperDisplay.displayShopWindow(shopSystem, playerInventory);
    }

    private void removeWindow() {
        shopKeeperDisplay.gameObject.SetActive(false);  
        questGiverDisplayMain.SetActive(false);      
    }

    private void displayQuestWindow(Quest quest) {
        questGiverDisplayMain.SetActive(true);
        questGiverDisplay.displayQuestWindow(quest);
    }
}
