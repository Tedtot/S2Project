using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public abstract class InventoryHolder : MonoBehaviour
{
    [SerializeField] private int invSize;
    [SerializeField] protected InventorySystem primaryInvSystem;
    [SerializeField] protected int offset = 9;
    [SerializeField] protected int gold;

    public int Offset => offset;
    public int InvSize => invSize;
    public InventorySystem PrimaryInvSystem => primaryInvSystem; 
    public static UnityAction<InventorySystem, int> OnDynamicInvetoryDisplayRequested; //Inv system to display, amount to offset display by

    protected virtual void Awake() {
        SaveLoad.OnLoadGame += loadInventory;
        primaryInvSystem = new InventorySystem(invSize, gold);
    }
    
    protected abstract void loadInventory(SaveData data);
}

[System.Serializable]
public struct invSaveData
{
    public InventorySystem InvSystem;
    public Vector3 Position;
    public Quaternion Rotation;

    public invSaveData(InventorySystem invSystem, Vector3 position, Quaternion rotation) {
        this.InvSystem = invSystem;
        this.Position = position;
        this.Rotation = rotation;
    }
    public invSaveData(InventorySystem invSystem) {
        this.InvSystem = invSystem;
        Position = Vector3.zero;
        Rotation = Quaternion.identity;
    }
}
