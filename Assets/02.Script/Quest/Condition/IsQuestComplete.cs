using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Quest/Condition/CompleteCondition", fileName ="CompleteCondition_")]
public class IsQuestComplete : Condition
{
    [SerializeField]
    private Quest target;

    public override bool IsPass(Quest quest)
    {
        return QuestSystem.Instance.ContainsCompletedQuest(target);
    }
}