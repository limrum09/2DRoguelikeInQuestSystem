using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementView : MonoBehaviour
{
    [SerializeField]
    private GameObject content;
    [SerializeField]
    private AchievementElement element;

    private RectTransform contentRect;

    private int elementCount;
    private void Start()
    {
        contentRect = content.GetComponent<RectTransform>();
        var achievements = QuestSystem.Instance;
        elementCount = 0;

        foreach(var achievement in achievements.ActiveAchievements)
        {
            AddElement(achievement, AchivementCondition.Running);
            elementCount++;
        }

        foreach(var achievement in achievements.CompletedAchievements)
        {
            AddElement(achievement, AchivementCondition.Complete);
            elementCount++;
        }

        Vector2 size = contentRect.sizeDelta;
        size.y = element.gameObject.GetComponent<RectTransform>().sizeDelta.y * elementCount + 5.0f;
        contentRect.sizeDelta = size;

        this.gameObject.SetActive(false);
    }

    public void AddElement(Quest achievement, AchivementCondition condition)
    {
        var newAchievement = Instantiate(element, content.transform);

        newAchievement.AchievementRegister(achievement, condition);

        QuestSystem.Instance.onAchievementsCompleted += newAchievement.AchievementCompleted;
    }
}
