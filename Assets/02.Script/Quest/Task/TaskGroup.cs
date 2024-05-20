using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum TaskGroupState
{
    Inactive,
    Running,
    Complete
}

// MonoBehaviour이나, ScriptableObject등 상속받은 class가 없기에, 다른 cript에서 [SerializeField]로 호출시, 사용할 수가 없다.
[System.Serializable]
public class TaskGroup
{
    [SerializeField]
    private Task[] tasks;

    public IReadOnlyList<Task> Tasks => tasks;
    public Quest Owner { get; private set; }
    public bool IsAllTaskComplete => tasks.All(x => x.IsComplete);
    public bool IsComplete => State == TaskGroupState.Complete;
    
    public TaskGroupState State { get; private set; }
    public TaskGroup(TaskGroup copyTarget)
    {
        tasks = copyTarget.Tasks.Select(x => Object.Instantiate(x)).ToArray();
    }


    public void TaskGroupSetUP(Quest owner)
    {
        Owner = owner;
        foreach(var task in tasks)
        {
            task.TaskSetup(owner);
        }

        if (IsAllTaskComplete)
        {
            State = TaskGroupState.Complete;
        }
            
    }

    public void TaskGroupStart()
    {
        if (IsAllTaskComplete)
        {
            State = TaskGroupState.Complete;
            return;
        }

        State = TaskGroupState.Running;
        foreach(var task in tasks)
        {
            task.TaskStart();
        }            
    }

    public void TaskGroupEnd()
    {
        State = TaskGroupState.Complete;
        foreach(var task in tasks)
        {
            task.TaskEnd();
        }
    }

    public void TaskGroupRecieveRoport(string category, object target, int successCount)
    {
        foreach(var task in tasks)
        {
            if(task.TaskIsTarget(category, target))
            {
                task.TaskRecieveReport(successCount);
            }
        }
    }

    public void TaskGroupComplete()
    {
        if (IsComplete)
        {
            return;
        }

        State = TaskGroupState.Complete;
        foreach(var task in tasks)
        {
            if (!task.IsComplete)
            {
                task.TaskComplete();
            }
        }
    }

    public bool TaskGroupContains(object target) => tasks.Any(x => x.TaskContainsTarget(target));
    public bool TaskGroupContains(TaskTarget target) => TaskGroupContains(target.Value);
}
