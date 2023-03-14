using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;

[CreateAssetMenu(menuName = "Inventory System/Item Database")]
public class Database : ScriptableObject
{
    [SerializeField] private List<InvItemData> itemDatabase; //List of all items
    [ContextMenu("Set IDs")]
    public void setItemIDs() {
        itemDatabase = new List<InvItemData>();
        var foundItems = Resources.LoadAll<InvItemData>("ItemData").OrderBy(i => i.ID).ToList(); //i: item

        var hasIDInRange = foundItems.Where(i => i.ID != -1 && i.ID < foundItems.Count).OrderBy(i => i.ID).ToList();
        var hasIDNotInRange = foundItems.Where(i => i.ID != -1 && i.ID >= foundItems.Count).OrderBy(i => i.ID).ToList();
        var noID = foundItems.Where(i => i.ID <= -1).OrderBy(i => i.ID).ToList();

        var index = 0;
        for (int i = 0; i < foundItems.Count; i++) {
            InvItemData itemToAdd;
            itemToAdd = hasIDInRange.Find(d => d.ID == i);

            if (itemToAdd != null) {
                itemDatabase.Add(itemToAdd);
            } 
            else if (index < noID.Count) {
                noID[index].ID = i;
                itemToAdd = noID[index];
                index++;
                itemDatabase.Add(itemToAdd);
            }
#if UNITY_EDITOR
    if (itemToAdd) EditorUtility.SetDirty(itemToAdd);
#endif

    }
        foreach (var item in hasIDNotInRange) {
                itemDatabase.Add(item);
#if UNITY_EDITOR
    if (item) EditorUtility.SetDirty(item);
#endif
        }
#if UNITY_EDITOR
    AssetDatabase.SaveAssets();
#endif
    }

    public InvItemData getItem(int id) {
        return itemDatabase.Find(i => i.ID == id);
    }
}
