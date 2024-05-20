using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentQuestView : MonoBehaviour
{
    [SerializeField]
    private GameObject currentQuests;
    [SerializeField]
    private GameObject questViewPrefab;
    [SerializeField]
    private CurrentQuest currentQuestPrafab;


    private void Start()
    {
        QuestSystem.Instance.onQuestRegistered += CreateQeusts;
        foreach(var quest in QuestSystem.Instance.ActiveQuests)
        {
            CreateQeusts(quest);
        }
    }

    private void OnDestroy()
    {
      //  QuestSystem.Instance.onQuestRegistered -= CreateQeusts;
    }

    private void CreateQeusts(Quest quest)
    {
        if (quest.IsComplete)
            return;
        else
            Instantiate(currentQuestPrafab, currentQuests.transform).GetQuest(quest);
    }
}
