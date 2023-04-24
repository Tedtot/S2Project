using UnityEngine;
using UnityEngine.InputSystem;

public class HotbarDisplay : StaticInventoryDisplay
{
    private int maxIndexSize = 8;
    private int currentIndex = 0;

    private PlayerControls playerControls;

    private void Awake() {
        playerControls = new PlayerControls();
    }

    protected override void Start() {
        base.Start();

        currentIndex = 0;
        maxIndexSize = slots.Length - 1;

        slots[currentIndex].toggleHighlight();
    }

    protected override void OnEnable() {
        base.OnEnable();
        playerControls.Enable();

        var player = playerControls.player;

        player.hotbar1.performed += hotbar1;
        player.hotbar2.performed += hotbar2;
        player.hotbar3.performed += hotbar3;
        player.hotbar4.performed += hotbar4;
        player.hotbar5.performed += hotbar5;
        player.hotbar6.performed += hotbar6;
        player.hotbar7.performed += hotbar7;
        player.hotbar8.performed += hotbar8;
        player.hotbar9.performed += hotbar9;
        player.useitem.performed += useItem;
    }

    protected override void OnDisable() {
        base.OnDisable();
        playerControls.Enable();

        var player = playerControls.player;

        player.hotbar1.performed -= hotbar1;
        player.hotbar2.performed -= hotbar2;
        player.hotbar3.performed -= hotbar3;
        player.hotbar4.performed -= hotbar4;
        player.hotbar5.performed -= hotbar5;
        player.hotbar6.performed -= hotbar6;
        player.hotbar7.performed -= hotbar7;
        player.hotbar8.performed -= hotbar8;
        player.hotbar9.performed -= hotbar9;
        player.useitem.performed -= useItem;
    }

    private void Update() {
        var player = playerControls.player;

        if (player.mousewheel.ReadValue<float>() > 0.1f) changeIndex(1);
        if (player.mousewheel.ReadValue<float>() < -0.1f) changeIndex(-1);
    }

    #region Hotbar Select Method
    
    private void hotbar1(InputAction.CallbackContext obj) { setIndex(0); }
    private void hotbar2(InputAction.CallbackContext obj) { setIndex(1); }
    private void hotbar3(InputAction.CallbackContext obj) { setIndex(2); }
    private void hotbar4(InputAction.CallbackContext obj) { setIndex(3); }
    private void hotbar5(InputAction.CallbackContext obj) { setIndex(4); }
    private void hotbar6(InputAction.CallbackContext obj) { setIndex(5); }
    private void hotbar7(InputAction.CallbackContext obj) { setIndex(6); }
    private void hotbar8(InputAction.CallbackContext obj) { setIndex(7); }
    private void hotbar9(InputAction.CallbackContext obj) { setIndex(8); }

    #endregion

    private void useItem(InputAction.CallbackContext obj) {
        if (slots[currentIndex].AssignedSlot.ItemData != null) slots[currentIndex].AssignedSlot.ItemData.useItem(slots[currentIndex]);
    }

    private void changeIndex(int direction) {
        slots[currentIndex].toggleHighlight();
        currentIndex += direction;

        if (currentIndex > maxIndexSize) currentIndex = 0;
        if (currentIndex < 0) currentIndex = maxIndexSize;

        slots[currentIndex].toggleHighlight();
    }

    private void setIndex(int newIndex) {
        slots[currentIndex].toggleHighlight();
        if (newIndex < 0) currentIndex = 0;
        if (newIndex > maxIndexSize) newIndex = maxIndexSize;

        currentIndex = newIndex;
        slots[currentIndex].toggleHighlight();
    }  

}
