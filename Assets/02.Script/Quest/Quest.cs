using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum QuestState
{
    Inactive,
    Running,
    WailtForComplete,
    Complete,
    Cancel
}
[CreateAssetMenu(menuName ="Quest/Quest",fileName ="Quest_")]
public class Quest : ScriptableObject
{
    #region Events
    public delegate void TaskSuccessChangeHandler(Quest quest, Task task, int currentSuccess, int prevSuccess);
    public delegate void CompletedHandler(Quest quest);
    public delegate void CanceledHandler(Quest quest);
    public delegate void NewTaskGroupHandler(Quest quest, TaskGroup currentTaskGroup, TaskGroup prevTaskGroup);
    #endregion

    [SerializeField]
    private Category category;
    [SerializeField]
    private Sprite icon;

    [Header("Text")]
    [SerializeField]
    private string codeName;
    [SerializeField]
    private string displayName;

    [Header("Task")]
    [SerializeField]
    private TaskGroup[] taskGroups;

    [Header("Reward")]
    [SerializeField]
    private Reward[] rewards;

    [Header("Option")]
    [SerializeField]
    private bool useAutoComplete;
    [SerializeField]
    private bool isCancelable;
    [SerializeField]
    private bool isSaveable;

    [Header("Condition")]
    [SerializeField]
    private Condition[] acceptionConditions;
    [SerializeField]
    private Condition[] cancelConditions;

    private int currentTaskGourpIndex;

    public event TaskSuccessChangeHandler onTaskSuccessChanged;
    public event CompletedHandler onQuestCompleted;
    public event CanceledHandler onQuestCanceled;
    public event NewTaskGroupHandler onNewTaskGroup;

    public Category Category => category;
    public Sprite Icon => icon;
    public string CodeName => codeName;
    public string DisplayName => displayName;
    public QuestState State { get; private set; }
    public TaskGroup CurrentTaskGroup => taskGroups[currentTaskGourpIndex];
    public IReadOnlyList<TaskGroup> TaskGroup => taskGroups;
    public IReadOnlyList<Reward> Reward => rewards;
    public bool IsRegisterd => State != QuestState.Inactive;
    public bool IsComplete => State == QuestState.Complete;
    public bool IsCompletable => State == QuestState.WailtForComplete;
    public bool IsCancel => State == QuestState.Cancel;
    public virtual bool IsCancelable => IsCancel && cancelConditions.All(x => x.IsPass(this));
    public bool IsAcceiption => acceptionConditions.All(x => x.IsPass(this));
    public virtual bool IsSavable => isSaveable;

    public void QuestOnRegister()
    {
        foreach(var taskGroup in taskGroups)
        {
            taskGroup.TaskGroupSetUP(this);
            foreach(var task in taskGroup.Tasks)
            {
                task.onSuccessChange += QuestOnSuccessChange;
            }
        }

        State = QuestState.Running;
        CurrentTaskGroup.TaskGroupStart();
    }

    public void QuestRecieveReport(string category, object target, int successCount)
    {
        if (IsComplete)
        {
            return;
        }
        CurrentTaskGroup.TaskGroupRecieveRoport(category, target, successCount);

        if (CurrentTaskGroup.IsAllTaskComplete)
        {
            if(currentTaskGourpIndex + 1 == taskGroups.Length)
            {
                State = QuestState.WailtForComplete;

                if (useAutoComplete)
                {
                    Debug.Log("UseAutoComplete");
                    QuestComplete();
                }
            }
            else
            {
                var prevTaskGroup = taskGroups[currentTaskGourpIndex++];
                prevTaskGroup.TaskGroupEnd();
                CurrentTaskGroup.TaskGroupStart();
                onNewTaskGroup?.Invoke(this, CurrentTaskGroup, prevTaskGroup);
            }
        }
        else
        {
            State = QuestState.Running;
        }
    }

    public void QuestComplete()
    {
        Debug.Log("QuestComplete");
        foreach(var taskGroup in taskGroups)
        {
            taskGroup.TaskGroupComplete();
        }

        State = QuestState.Complete;

        foreach(var reward in rewards)
        {
            reward.Give(this);
        }

        onQuestCompleted?.Invoke(this);

        onTaskSuccessChanged = null;
        onQuestCompleted = null;
        onQuestCanceled = null;
        onNewTaskGroup = null;
    }

    public virtual void QuestCancel()
    {
        State = QuestState.Cancel;
        onQuestCanceled?.Invoke(this);
    }

    public void QuestContains(object target) => taskGroups.Any(x => x.TaskGroupContains(target));
    public void QuestContains(TaskTarget target) => QuestContains(target.Value);

    public Quest Clone()
    {
        // 지금은 Task만 복사햐서 만들어 넣어준다.
        // 만약, 다른 Moduel에서 실시간으로 변수 값이 바뀌는 경우가 있다면 이렇게 만들어줘야한다.
        var clone = Instantiate(this);
        clone.taskGroups = taskGroups.Select(x => new TaskGroup(x)).ToArray();

        return clone;
    }

    private void QuestOnSuccessChange(Task task, int currentSuccess, int prevSuccess)
        => onTaskSuccessChanged?.Invoke(this, task, currentSuccess, prevSuccess);

    public QuestSaveData ToSaveData()
    {
        return new QuestSaveData
        {
            codeName = codeName,
            state = State,
            taskGroupIndex = currentTaskGourpIndex,
            taskSuccessCount = CurrentTaskGroup.Tasks.Select(x => x.currentSuccess).ToArray()
        };
    }

    public void LoadFrom(QuestSaveData saveData)
    {
        State = saveData.state;
        currentTaskGourpIndex = saveData.taskGroupIndex;

        for(int i = 0; i < currentTaskGourpIndex; i++)
        {
            var taskGroup = taskGroups[i];
            taskGroup.TaskGroupStart();
            taskGroup.TaskGroupComplete();
        }

        for(int i = 0; i < saveData.taskSuccessCount.Length; i++)
        {
            CurrentTaskGroup.TaskGroupStart();
            CurrentTaskGroup.Tasks[i].CurrentSuccess = saveData.taskSuccessCount[i];
        }
    }
}
