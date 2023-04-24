using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest Data")]
public class Quest : ScriptableObject
{
    public string QuestName;
    public string QuestDescription;
    public int Gold;
    public int Experience;
    public List<int> Amount;
    public List<int> CompletedAmount;
    public List<InventoryItemData> ItemData;

    public Dictionary<InventoryItemData, int> QuestData;

    public Quest(string questName, string questDescription, int gold, int experience, List<int> amount, List<InventoryItemData> itemData) {
        this.QuestName = questName;
        this.QuestDescription = questDescription;
        this.Gold = gold;
        this.Experience = experience;
        this.Amount = amount;
        setCompletedAmount(itemData.Count);
        this.ItemData = itemData;

        QuestData = new Dictionary<InventoryItemData, int>();

        for (int i = 0; i < itemData.Count; i++) {
            QuestData.Add(itemData[i], amount[i]);
        }
    }

    public Quest(Quest quest) {
        this.QuestName = quest.QuestName;
        this.QuestDescription = quest.QuestDescription;
        this.Gold = quest.Gold;
        this.Experience = quest.Experience;
        this.Amount = quest.Amount;
        setCompletedAmount(quest.ItemData.Count);
        this.ItemData = quest.ItemData;
    }

    public Quest() {
        this.QuestName = "";
        this.QuestDescription = "";
        this.Gold = 0;
        this.Experience = 0;
        this.Amount = new List<int>();
        this.CompletedAmount = new List<int>();
        this.ItemData = new List<InventoryItemData>();
    }

    private void setCompletedAmount(int count) {
        this.CompletedAmount = new List<int>(count);
        for (int i = 0; i < count; i++) this.CompletedAmount.Add(0);

    }
}
