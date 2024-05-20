using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    [SerializeField]
    private List<Quest> quests;

    private void Start()
    {        
        var obj = FindObjectsOfType<QuestGiver>();
        if(obj.Length == 1)
        {
            DontDestroyOnLoad(gameObject);

            foreach (var quest in quests)
            {
                if (!QuestSystem.Instance.ContainsCompletedQuest(quest) && quest.IsAcceiption)
                {
                    Debug.Log("Quest Giver To QuestSystem : " + quest + ", " + quest.State);
                    QuestSystem.Instance.QuestSystemRegister(quest);
                }
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public int QuestAdd(Quest quest)
    {
        quests.Add(quest);

        int questIndex = quests.Count - 1;

        QuestSystem.Instance.QuestSystemRegister(quests[questIndex]);

        return questIndex;
    }
}
