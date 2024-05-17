using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class QuestListViewController : MonoBehaviour
{
    [SerializeField]
    private ToggleGroup tabGroup;
    [SerializeField]
    private QuestList activeQuest;
    [SerializeField]
    private QuestList completedQuest;

    public IEnumerable<Toggle> Tabs => tabGroup.ActiveToggles();

    public void AddQuestToActiveListView(Quest quest, UnityAction<bool> onClicked) => activeQuest.AddElement(quest, onClicked);
    public void RemoveQeustFromActiveListView(Quest quest) => activeQuest.RemoveElement(quest);
    public void AddQeustToCompletedListView(Quest quest, UnityAction<bool> onClicked) => completedQuest.AddElement(quest, onClicked);
    
}
