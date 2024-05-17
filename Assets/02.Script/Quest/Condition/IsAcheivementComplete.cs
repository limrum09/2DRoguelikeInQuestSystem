using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Condition/AchievementCompleteCondition", fileName = "AchievementCompleteCondition_")]
public class IsAcheivementComplete : Condition
{
    [SerializeField]
    private Achievement achievement;
    public override bool IsPass(Quest quest)
    {
        return QuestSystem.Instance.ContainsCompeltedAchievement(achievement);
    }
}
