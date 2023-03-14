using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData
{
    public List<string> collectedItems;
    public SerializableDictionary<string, itemPickupSaveData> activeItems;
    public SerializableDictionary<string, invSaveData> chestDictionary;

    public invSaveData playerInventory;
    public SaveData() {
        collectedItems = new List<string>();
        activeItems = new SerializableDictionary<string, itemPickupSaveData>();
        chestDictionary = new SerializableDictionary<string, invSaveData>();
        playerInventory = new invSaveData();
    }

}
