using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    [SerializeField] private Button loadButton;
    [SerializeField] private Button newSaveButton;
    [SerializeField] private Button testButton;

    [SerializeField] private GameObject background;
    [SerializeField] private GameObject mainMenu;

    [SerializeField] private PlayerMovement pm;

    private void Start() {
        loadButton.onClick.AddListener(tryLoadData);
        newSaveButton.onClick.AddListener(newData);
        testButton.onClick.AddListener(closeMenu);

        openMenu();
    }

    public void tryLoadData() {
        SaveGameManager.tryLoadData();
        closeMenu();
    }

    public void newData() {
        SaveGameManager.deleteData();
        closeMenu();
    }

    public void closeMenu() {
        mainMenu.SetActive(false);
        background.SetActive(false);

        pm.mainMenuOpen(false);
    }

    public void openMenu() {
        mainMenu.SetActive(true);
        background.SetActive(true);

        pm.mainMenuOpen(true);
    }
}
