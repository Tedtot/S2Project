using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGameManager : MonoBehaviour
{
    public static SaveData data;
    private void Awake() {
        data = new SaveData();
        SaveLoad.OnLoadGame += loadData;
    }
    public static void deleteData() {
        SaveLoad.deleteSaveData();
    }

    public static void saveData() {
        var saveData = data;
        SaveLoad.save(saveData);
    }

    public static void loadData(SaveData _data) {
        data = _data;
    }

    public static void tryLoadData() {
        SaveLoad.load();
    }
}
