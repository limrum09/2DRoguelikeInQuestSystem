using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestDetailView : MonoBehaviour
{
    [SerializeField]
    private GameObject displayGroup;
    [SerializeField]
    private Button cancelButton;

    [Header("Quest")]
    [SerializeField]
    private TextMeshProUGUI title;
    [SerializeField]
    private TextMeshProUGUI description;

    [Header("Task")]
    [SerializeField]
    private RectTransform taskDescriptionGroup;
    [SerializeField]
    private QuestDetailviewDescription taskDescriptionPrefab;
    [SerializeField]
    private int taskDescriptionPoolCount;

    [Header("Reward")]
    [SerializeField]
    private RectTransform rewardDescriptionGroup;
    [SerializeField]
    private TextMeshProUGUI rewardDescriptionPrefab;
    [SerializeField]
    private int rewardDescriptionPoolCount;

    private List<QuestDetailviewDescription> taskDescriptionPool;
    private List<TextMeshProUGUI> rewardDescriptionPool;

    public Quest Target { get; private set; }

    private void Awake()
    {
        taskDescriptionPool = CreatePool(taskDescriptionPrefab, taskDescriptionPoolCount, taskDescriptionGroup);
        rewardDescriptionPool = CreatePool(rewardDescriptionPrefab, rewardDescriptionPoolCount, rewardDescriptionGroup);
    }
    private void Start()
    {
        cancelButton.onClick.AddListener(CancelQuest);
    }

    private void CancelQuest()
    {
        Debug.Log("Cancel Quest");            
    }

    private List<T> CreatePool<T>(T prefab, int count, RectTransform parent) where T : MonoBehaviour
    {
        // count �� ��ŭ parent�� ��ġ�� pool����
        var pool = new List<T>(count);

        for(int i = 0; i < count; i++)
        {
            pool.Add(Instantiate(prefab, parent));
        }

        return pool;
    }

    public void Show(Quest quest)
    {
        Debug.Log(quest);

        if (!quest)
        {
            displayGroup.SetActive(false);
            return;
        }
        displayGroup.SetActive(true);
        Target = quest;

        // Quest Description �κ�
        title.text = quest.CodeName;
        description.text = quest.DisplayName;

        // Task Description �κ�
        int taskIndex = 0;
        foreach(var taskGroup in quest.TaskGroup)
        {
            // ���� task�� ����Ȯ��
            foreach(var task in taskGroup.Tasks)
            {
                // task�� �ִٸ� PoolObject�� ������� ���
                var poolObject = taskDescriptionPool[taskIndex++];
                poolObject.gameObject.SetActive(true);

                if (taskGroup.IsComplete)
                    poolObject.UpdateTexkUsingStrikeThrough(task);
                else if (taskGroup == quest.CurrentTaskGroup)
                    poolObject.UpdateText(task);
                else
                    poolObject.UpdateText("?????????");
            }
        }

        // ������� �ʴ� Pool���� ���ش�.
        for(int i = taskIndex; i < taskDescriptionPool.Count; i++)
        {
            taskDescriptionPool[i].gameObject.SetActive(false);
        }

        // Reward Description �κ�
        var rewards = quest.Reward;
        var rewardCount = rewards.Count;
        for(int i = 0; i < rewardDescriptionPoolCount; i++)
        {
            var poolObject = rewardDescriptionPool[i];
            if (i < rewardCount)
            {
                var reward = rewards[i];
                poolObject.text = $"{reward.Description} + {reward.Value}";
                poolObject.gameObject.SetActive(true);
            }
            else
                poolObject.gameObject.SetActive(false);
        }

        // ��Ұ����ϰ� �Ϸ���� ���� ����Ʈ��� ��� ��ư Ȱ��ȭ
        cancelButton.gameObject.SetActive(quest.IsCancelable && !quest.IsComplete);
    }

    public void Hide()
    {
        Target = null;
        displayGroup.SetActive(false);
        cancelButton.gameObject.SetActive(false);
    }
}
