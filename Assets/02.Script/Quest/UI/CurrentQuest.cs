using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrentQuest : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI questTitle;
    [SerializeField]
    private QuestContent questContent;
    [SerializeField]
    private Color normalColor;
    [SerializeField]
    private Color activeQuestCountColor;
    [SerializeField]
    private Color completeQuestColor;

    private Dictionary<Task, QuestContent> questContentByTask = new Dictionary<Task, QuestContent>();

    private Quest targetQuest;

    private void OnDestroy()
    {
        if (targetQuest != null)
        {
            targetQuest.onNewTaskGroup -= UpdateTaskGroup;
            targetQuest.onQuestCompleted -= DestroySelf;
        }
        
        foreach (var tuple in questContentByTask)
        {
            var task = tuple.Key;
            task.onSuccessChange -= UpdateText;
        }
    }
    public void GetQuest(Quest quest)
    {
        this.targetQuest = quest;
        Debug.Log("================Get Qeust");
        questTitle.text = $"[{quest.Category.CodeName}] {quest.CodeName}";

        IReadOnlyList<TaskGroup> groups = quest.TaskGroup;

        UpdateTaskGroup(quest, groups[0]);
        quest.onNewTaskGroup += UpdateTaskGroup;
        quest.onQuestCompleted += Destroy;

        if(groups[0] != quest.CurrentTaskGroup)
        {
            for (int i = 1; i < groups.Count; i++)
            {
                var taskGroup = groups[i];
                UpdateTaskGroup(quest, taskGroup, groups[i - 1]);

                if (taskGroup == quest.CurrentTaskGroup) break;
            }
        }
    }

    private void UpdateTaskGroup(Quest quest, TaskGroup currentTaskGroup, TaskGroup prevTaskGroup = null)
    {
        Debug.Log("***********UpdateTaskGroup");
        foreach (var task in currentTaskGroup.Tasks)
        {
            var taskDescription = Instantiate(questContent, this.transform);
            SetText(task, taskDescription);
            task.onSuccessChange += UpdateText;

            questContentByTask.Add(task, taskDescription);
        }

        if(prevTaskGroup != null)
        {
            foreach(var task in prevTaskGroup.Tasks)
            {
                var taskDescription = questContentByTask[task];
                taskDescription.TextStrikeThrough(task);

                SetText(task, taskDescription);
            }
        }
    }

    private void UpdateText(Task task, int currentSueccss, int prevSuccess)
    {
        var content = questContentByTask[task];
        SetText(task, content);
    }

    public void SetText(Task task, QuestContent content)
    {
        var normalColorString = ColorUtility.ToHtmlStringRGB(normalColor);
        var activeColorString = ColorUtility.ToHtmlStringRGB(activeQuestCountColor);
        var completeColorString = ColorUtility.ToHtmlStringRGB(completeQuestColor);

        content.UpdateText(task, normalColorString, activeColorString, completeColorString);
    }

    private void DestroySelf(Quest quest)
    {
        Debug.Log("DestroySelf"); 
        Destroy(gameObject);
    }
}
