using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class QuestList : MonoBehaviour
{
    [SerializeField]
    private GameObject content;
    [SerializeField]
    private TextMeshProUGUI elementTextPrefab;

    private Dictionary<Quest, GameObject> elementQuest = new Dictionary<Quest, GameObject>();
    private ToggleGroup toggleGroup;

    private void Start()
    {
        toggleGroup = GetComponent<ToggleGroup>();
    }

    public void AddElement(Quest quest, UnityAction<bool> onClicked)
    {
        var element = Instantiate(elementTextPrefab, content.transform);
        element.text = quest.DisplayName;

        var toggle = element.GetComponent<Toggle>();
        toggle.group = toggleGroup;
        toggle.onValueChanged.AddListener(onClicked);

        elementQuest.Add(quest, element.gameObject);
    }

    public void RemoveElement(Quest quest)
    {
        Destroy(elementQuest[quest]);
        elementQuest.Remove(quest);
    }
}
