using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestDetailviewDescription : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;

    [SerializeField]
    private Color normalText;
    [SerializeField]
    private Color completedText;
    [SerializeField]
    private Color currentCountText;
    [SerializeField]
    private Color strikeThroughText;

    public void UpdateText(string text)
    {
        this.text.fontStyle = FontStyles.Normal;
        this.text.text = text;
    }

    public void UpdateText(Task task)
    {
        text.fontStyle = FontStyles.Normal;

        if (task.IsComplete)
        {
            var code = ColorUtility.ToHtmlStringRGB(completedText);
            text.text = BuildText(task, code, code);
        }
        else
        {
            var normalCode = ColorUtility.ToHtmlStringRGB(normalText);
            var countCode = ColorUtility.ToHtmlStringRGB(currentCountText);
            text.text = BuildText(task, normalCode, countCode);
        }
    }

    public void UpdateTexkUsingStrikeThrough(Task task)
    {
        var colorCode = ColorUtility.ToHtmlStringRGB(strikeThroughText);
        text.fontStyle = FontStyles.Strikethrough;
        text.text = BuildText(task, colorCode, colorCode);
    }

    private string BuildText(Task task,string textColotCode, string successCountColorCode)
    {
        return $"<color=#{textColotCode}> {task.Description} <color=#{successCountColorCode}>{task.CurrentSuccess}</color>/{task.NeedSuccessCount}</color>";
    }
}
