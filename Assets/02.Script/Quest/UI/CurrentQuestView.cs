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
        Debug.Log("Current Quest View Start");

        QuestSystem.Instance.onQuestRegistered += CreateQeusts;
        foreach(var quest in QuestSystem.Instance.ActiveQuests)
        {
            Debug.Log("Start Create Quest");
            CreateQeusts(quest);
        }
    }

    private void OnDestroy()
    {
      //  QuestSystem.Instance.onQuestRegistered -= CreateQeusts;
    }

    private void CreateQeusts(Quest quest)
    {
        Debug.Log("Create Quest");
        if (quest.IsComplete)
            return;
        else
            Instantiate(currentQuestPrafab, currentQuests.transform).GetQuest(quest);
    }
}
