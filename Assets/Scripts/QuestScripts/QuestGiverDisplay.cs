using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.UI;


public class QuestGiverDisplay : MonoBehaviour
{
    [Header("TMPTextObjects")]
    [SerializeField] private TextMeshProUGUI questNameText;
    [SerializeField] private TextMeshProUGUI questDescriptionText;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI experienceText;
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private TextMeshProUGUI playerQuestTitle;
    [SerializeField] private TextMeshProUGUI playerGoldText;

    [Header("GameObjects")]
    [SerializeField] private GameObject questList;
    [SerializeField] private GameObject questPrefab;
    [SerializeField] private GameObject questListContentPanel;

    [Header("Other")]
    [SerializeField] private Button questButton;
    [SerializeField] private List<Quest> quests;
    [SerializeField] private int questNumber;

    private bool activeQuest;
    private Quest quest;
    private string plural;

    public bool ActiveQuest => activeQuest;

    private PlayerInventoryHolder playerInvHolder;

    private void Start() {
        questNumber = 1;   
        activeQuest = false;

        quest = new Quest();
        plural = "";

        playerInvHolder =  GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventoryHolder>();

        foreach (var item in questListContentPanel.transform.Cast<Transform>()) Destroy(item.gameObject);
    }
 
    public void displayQuestWindow(Quest quest) {
        if (this.quest.ItemData.Count == 0) this.quest = new Quest(quest);

        questNameText.text = $"Quest {questNumber}: {quest.QuestName}";
        questDescriptionText.text = quest.QuestDescription;
        goldText.text = quest.Gold.ToString() + " G";
        experienceText.text = quest.Experience.ToString() + " EXP";
    }

    public void questStatus() {
        if (activeQuest == false) {
            activeQuest = true;

            playerQuestTitle.text = "Quest " + questNumber + ": " + quest.QuestName;
            for (int i = 0; i < quest.ItemData.Count; i++) {
                var newQuestItem = Instantiate(questPrefab, questListContentPanel.transform);
                if (quest.Amount[i] > 1) plural = "s";
                newQuestItem.GetComponent<TextMeshProUGUI>().text = 
                    $"Gather {quest.Amount[i]} {quest.ItemData[i].DisplayName}{plural}: {quest.CompletedAmount[i]}/{quest.Amount[i]}";
            }

            buttonText.text = "Complete Quest";
        }
        
        else {
            for (int i = 0; i < quest.ItemData.Count; i++) {
                if (quest.CompletedAmount[i] < quest.Amount[i]) { Debug.Log("Quest requirements not met!"); return; } 
            }

            activeQuest = false;

            playerInvHolder.PrimaryInvSystem.gainGold(quest.Gold);
            playerGoldText.text = playerInvHolder.PrimaryInvSystem.Gold.ToString() + " G";


            playerQuestTitle.text = "NO ACTIVE QUESTS";
            foreach (var item in questListContentPanel.transform.Cast<Transform>()) Destroy(item.gameObject);
            quest = new Quest(); //change this section later - the player just gets an empty quest after completion

            buttonText.text = "Accept Quest";
        }
    }

    public void updateQuestValue(int i) {
        quest.CompletedAmount[i]++;

        if (quest.Amount[i] > 1) plural = "s";
        questListContentPanel.transform.GetChild(i).gameObject.GetComponent<TextMeshProUGUI>().text = 
            $"Gather {quest.Amount[i]} {quest.ItemData[i].DisplayName}{plural}: {quest.CompletedAmount[i]}/{quest.Amount[i]}";
    }

    public Quest getQuestItems() {
        return quest;
    }

    public Quest newQuest() {
        /*
        int amount = Random.Range(1, 10);
        string questName = "Collect " + amount + "";
        return null;*/
        var quest = Resources.Load<Quest>("Quests/Quest1");
        return quest;
    }
}
