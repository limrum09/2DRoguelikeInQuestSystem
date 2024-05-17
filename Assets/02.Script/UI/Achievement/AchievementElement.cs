using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum AchivementCondition
{
    Running,
    Complete
}

public class AchievementElement : MonoBehaviour
{
    [SerializeField]
    private Color completedColor;
    [SerializeField]
    private Image icon;
    [SerializeField]
    private TextMeshProUGUI codeName;
    [SerializeField]
    private TextMeshProUGUI displayName;
    [SerializeField]
    private CountAndReward countAndReward;

    private AchivementCondition condition;

    private Quest target;
    private bool onView;
    public TextMeshProUGUI CodeName => codeName;

    private void OnDestroy()
    {
        QuestSystem.Instance.onAchievementsCompleted -= AchievementCompleted;
    }

    public void AchievementRegister(Quest achievement, AchivementCondition inputCondition)
    {
        target = achievement;
        condition = inputCondition;

        CheckAchivementCondition(condition);

        icon.sprite = target.Icon;
        codeName.text = target.CodeName;
        displayName.text = target.DisplayName;

        Task achievementTask = target.TaskGroup[0].Tasks[0];
        countAndReward.SetTask(achievementTask);

        var rewards = target.Reward;
        foreach (var reward in rewards)
        {
            countAndReward.SetReward(reward);
        }
        achievementTask.onSuccessChange += countAndReward.RefreshText;
        IsViewAchievement();
    }

    public void IsViewAchievement()
    {
        onView = target.IsAcceiption;
        this.gameObject.SetActive(onView);
    }

    public void AchievementCompleted(Quest achievement)
    {
        if(target.IsComplete) condition = AchivementCondition.Complete;

        IsViewAchievement();
        CheckAchivementCondition(condition);
    }

    public void CheckAchivementCondition(AchivementCondition inputCondition)
    {
        condition = inputCondition;

        if (condition == AchivementCondition.Complete)
        {
            this.gameObject.GetComponent<Image>().color = completedColor;
        }
        else
        {
            this.gameObject.transform.SetAsFirstSibling();
        }
    }
}
