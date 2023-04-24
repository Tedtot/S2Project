using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData
{
    public List<string> CollectedItems;
    public SerializableDictionary<string, itemPickupSaveData> ActiveItems;
    public SerializableDictionary<string, invSaveData> ChestDictionary;
    public SerializableDictionary<string, ShopSaveData> ShopKeeperDictionary;

    public invSaveData playerInventory;
    public TimeSaveData TimeSaveData;

    public SaveData() {
        CollectedItems = new List<string>();
        ActiveItems = new SerializableDictionary<string, itemPickupSaveData>();
        ChestDictionary = new SerializableDictionary<string, invSaveData>();
        playerInventory = new invSaveData();
        ShopKeeperDictionary = new SerializableDictionary<string, ShopSaveData>();
        TimeSaveData = new TimeSaveData();
    }
}
