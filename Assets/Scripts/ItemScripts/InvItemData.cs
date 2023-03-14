using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory Item Data")]
public class InvItemData : ScriptableObject
{
    public int ID = -1;
    public string DisplayName;
    public int MaxStackSize;
    public Sprite Icon;
    public GameObject ItemPrefab;
}
