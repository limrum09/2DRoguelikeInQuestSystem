using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestView : MonoBehaviour
{
    [SerializeField]
    private QuestListViewController questListViewController;
    [SerializeField]
    private QuestDetailView questDetailView;

    private void Start()
    {
        var questSystem = QuestSystem.Instance;

        foreach(var quest in questSystem.ActiveQuests)
        {
            AddQuestToActiveListView(quest);
        }
        foreach (var quest in questSystem.CompletedQuest)
        {
            AddQuestToCompletedListView(quest);
        }


        questSystem.onQuestRegistered += AddQuestToActiveListView;

        questSystem.onQuestCompleted += RemoveQuestFromActiveListView;
        questSystem.onQuestCompleted += AddQuestToCompletedListView;
        questSystem.onQuestCompleted += HideDetailIfQuestCancel;

        questSystem.onQuestCanceled += HideDetailIfQuestCancel;
        questSystem.onQuestCanceled += RemoveQuestFromActiveListView;

        foreach(var tab in questListViewController.Tabs)
        {
            tab.onValueChanged.AddListener(HideDetail);
        }

        this.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        var questSystem = QuestSystem.Instance;

        if (questSystem)
        {
            questSystem.onQuestRegistered -= AddQuestToActiveListView;

            questSystem.onQuestCompleted -= RemoveQuestFromActiveListView;
            questSystem.onQuestCompleted -= AddQuestToCompletedListView;
            questSystem.onQuestCompleted -= HideDetailIfQuestCancel;

            questSystem.onQuestCanceled -= HideDetailIfQuestCancel;
            questSystem.onQuestCanceled -= RemoveQuestFromActiveListView;
        }
    }

    private void AddQuestToActiveListView(Quest quest) => questListViewController.AddQuestToActiveListView(quest, isOn => ShowDetail(isOn, quest));
    private void AddQuestToCompletedListView(Quest quest) => questListViewController.AddQeustToCompletedListView(quest, isOn => ShowDetail(isOn, quest));
    private void RemoveQuestFromActiveListView(Quest quest)
    {
        questListViewController.RemoveQeustFromActiveListView(quest);
        
    }
    private void HideDetailIfQuestCancel(Quest quest)
    {
        if(questDetailView.Target == quest)
        {
            questDetailView.Hide();
        }
    }
    private void ShowDetail (bool isOn, Quest quest)
    {
        if (isOn)
        {
            questDetailView.Show(quest);
        }
    }
    private void HideDetail(bool isOn)
    {
        if (!isOn)
        {
            questDetailView.Hide();
        }
    }

    private void OnEnable()
    {
        // 시작하자마자 제일 위에 있는 퀘스트 정보 보여주기
        if(questDetailView != null)
        {
            questDetailView.Show(questDetailView.Target);
        }
    }
}
