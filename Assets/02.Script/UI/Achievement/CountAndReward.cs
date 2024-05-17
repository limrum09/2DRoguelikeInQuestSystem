using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountAndReward : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI categoryAndCount;
    [SerializeField]
    private TextMeshProUGUI rewards;

    private Task target;

    private int rewardCount = 0;

    private void OnDestroy()
    {
        if(target != null)
        {
            target.onSuccessChange -= RefreshText;
        }
    }
    public  void SetTask(Task task)
    {
        target = task;
        categoryAndCount.text = $"{task.Category.CodeName} : {task.currentSuccess} / {task.NeedSuccessCount}";
    }

    public void RefreshText(Task task, int currentSuccess, int prevSuccess)
    {
        categoryAndCount.text = $"{task.Category.CodeName} : {task.currentSuccess} / {task.NeedSuccessCount}";
    }

    public void SetReward(Reward reward)
    {
        string rewardText;
        if (rewardCount != 0)
        {
            rewardText = ", ";
            rewards.text += rewardText;
        }
         rewardText = $"{reward.Description} + {reward.Value}";

        rewards.text += rewardText;

        rewardCount++;
    }
}
