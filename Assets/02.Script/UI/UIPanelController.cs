using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelController : MonoBehaviour
{
    private static UIPanelController instance;

    [SerializeField]
    private TalkPanelController talkPanel;
    [SerializeField]
    private QuestView questView;
    [SerializeField]
    private AchievementView achievementView;

    public static UIPanelController Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<UIPanelController>();
            }

            return instance;
        }
    }

    void Awake()
    {
        var obj = FindObjectsOfType<UIPanelController>();
        if (obj.Length == 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        HideTalkPanel();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            questView.gameObject.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            achievementView.gameObject.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            questView.gameObject.SetActive(false);
            achievementView.gameObject.SetActive(false);
        }
    }

    public void ShowTalkPanel(Quest quest, Talker questTalker)
    {
        Completed.GameManager.instance.ArrowKeysEnable = false;

        talkPanel.gameObject.SetActive(true);
        talkPanel.Register(quest, questTalker);
    }

    public void HideTalkPanel()
    {
        Completed.GameManager.instance.ArrowKeysEnable = true;

        talkPanel.gameObject.SetActive(false);
    }
}
