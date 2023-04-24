using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//[RequireComponent(typeof(UniqueID))]
public class QuestGiver : MonoBehaviour, IInteractable
{
    [SerializeField] private List<Quest> questList;

    public static UnityAction<Quest> OnQuestWindowRequested;
    public static UnityAction OnQuestWindowRemoved;

    //[SerializeField] private QuestGiverDisplay questGiverDisplay;

    public UnityAction<IInteractable> OnInteractionComplete { get; set; }

    public void interact(Interactor interactor, out bool interactSuccessful) {
        OnQuestWindowRequested?.Invoke(questList[0]);
        interactSuccessful = true;
    }

    public void endInteraction() {
        OnQuestWindowRemoved?.Invoke();
    }
}
//Add datasave later
