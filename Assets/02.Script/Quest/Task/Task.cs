using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum TaskState
{
    Inactive,
    Running,
    Complete
}

[CreateAssetMenu(menuName ="Quest/Task/Task", fileName ="Task")]
public class Task : ScriptableObject
{
    #region Events
    public delegate void StateChangeHandler(Task task, TaskState currentState, TaskState prevsState);
    public delegate void SuccessChangeHandler(Task task, int currentSuccess, int prevSuccess);
    #endregion

    [SerializeField]
    private Category category;

    [Header("Text")]
    [SerializeField]
    private string codeNmae;
    [SerializeField]
    private string description;

    [Header("Action")]
    [SerializeField]
    private TaskAction action;

    [Header("Target")]
    [SerializeField]
    private TaskTarget[] taskTargets;

    [Header("Option")]
    [SerializeField]
    private int needSuccessCount;       //  필요 성공 값
    [SerializeField]
    private bool countingDuringComplete; // 퀘스트 성공 이후 카운트

    public int currentSuccess;
    private TaskState state;

    public event StateChangeHandler onStateChange;
    public event SuccessChangeHandler onSuccessChange;

    public int CurrentSuccess
    {
        get => currentSuccess;
        set
        {
            int prevSuccess = currentSuccess;
            
            currentSuccess = Mathf.Clamp(value, 0, needSuccessCount);
            if(prevSuccess != currentSuccess)
            {
                state = currentSuccess == needSuccessCount ? TaskState.Complete : TaskState.Running;
                onSuccessChange?.Invoke(this, currentSuccess, prevSuccess);
            }
        }
    }

    public string CodeName => codeNmae;
    public string Description => description;
    public int NeedSuccessCount => needSuccessCount;
    public bool CountingDuringComplete => countingDuringComplete;
    public Category Category => category;
    public TaskState State
    {
        get => state;
        set
        {
            var prevState = state;
            state = value;
            onStateChange?.Invoke(this, state, prevState);
        }
    }

    public bool IsComplete => state == TaskState.Complete;


    public void TaskRecieveReport(int successCount)
    {
        CurrentSuccess = action.Run(this, currentSuccess, successCount);
    }

    public Quest TaskOwner { get; private set; }

    public void TaskSetup(Quest quest)
    {
        TaskOwner = quest;

        if(CurrentSuccess >= needSuccessCount)
        {
            TaskComplete();
        }
    }

    public void TaskStart()
    {
        state = TaskState.Running;        
    }

    public void TaskEnd()
    {
        onStateChange = null;
        onSuccessChange = null;
    }

    public void TaskComplete()
    {
        CurrentSuccess = needSuccessCount;

        state = TaskState.Complete;
    }

    public  bool TaskIsTarget(string category, object target)
    {
        if(Category.CodeName == category)
        {
            return true;
        }

        return false;
    }

    // Any() 메소드 : 데이터 집합의 요소 중 하나라도 특정 조건을 만족하면 true를 반환하고, 그렇지 않으면 false를 반환한다.
    // All() 메소드 : 데이터 집합의 모든 요소가 주어진 조건을 만족하면 true를 반환하고, 그렇지 않으면 false를 반환한다.
    // Contains() 메소드 : 데이터의 집합에서 특정 값이 포함되어 있다면 true를 반환하고, 그렇지 않으면 false를 반환한다.
    public bool TaskContainsTarget(object target) => taskTargets.Any(x => x.IsEqual(target));
}
