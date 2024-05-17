using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestContent : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI TaskDescription;

    public void TextStrikeThrough(Task task)
    {
        TaskDescription.fontStyle = FontStyles.Strikethrough;
    }

    public void UpdateText(Task task, string textColor, string activeCountColor, string completeCountString)
    {
        if (task.IsComplete)
        {
            TaskDescription.text = SetText(task, textColor, completeCountString);
        }
        else
        {
            TaskDescription.text = SetText(task, textColor, activeCountColor);
        }
    }

    private string SetText(Task task, string textColor, string countColor)
    {
        return $"<color=#{textColor}> {task.Description} <color=#{countColor}>{task.currentSuccess}</color> / {task.NeedSuccessCount}</color>";
    }
}
