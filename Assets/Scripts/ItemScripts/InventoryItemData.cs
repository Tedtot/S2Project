using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory Item Data")]
public class InventoryItemData : ScriptableObject
{
    public int ID = -1;
    public string DisplayName;
    public int MaxStackSize;
    public int Gold;
    public Sprite Icon;
    public GameObject ItemPrefab;
}
