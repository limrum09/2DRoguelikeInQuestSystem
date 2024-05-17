using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TalkPanelController : MonoBehaviour
{
    private Quest quest;
    [Header("QuestContent")]
    [SerializeField]
    public Talker questContent;

    [Header("Player")]
    [SerializeField]
    public GameObject playerPanel;
    [SerializeField]
    public TextMeshProUGUI playerTalkText;

    [Header("NPC")]
    [SerializeField]
    public GameObject npcPanel;
    [SerializeField]
    public Image npcImage;
    [SerializeField]
    public TextMeshProUGUI npcName;
    [SerializeField]
    public TextMeshProUGUI npcTalkText;

    [Header("Buttons")]
    [SerializeField]
    public Button[] nextTalkButtons;
    [SerializeField]
    public Button acceptionButton;
    [SerializeField]
    public Button cancelButton;

    private int talkIndex;

    public void Register(Quest quest, Talker questContent)
    {
        this.questContent = questContent;

        npcImage.sprite = questContent.npcImage;
        npcName.text = questContent.npcName;

        questContent.condition = TalkerConditon.Running;
        talkIndex = 0;

        InputQuest(quest);
        Talking();
    }

    public void AcceptionQuest()
    {
        questContent.condition = TalkerConditon.Acceiption;

        QuestGiver questGiver;
        if (GameObject.FindGameObjectWithTag("QuestGiver"))
        {
            questGiver = GameObject.FindGameObjectWithTag("QuestGiver").GetComponent<QuestGiver>();
            questGiver.QuestAdd(quest);
        }
        else
        {
            Debug.LogError("QuestGiver does not currently exist in Scene.");
            Debug.LogError("Add it temporarily from QuestSystem.");
            QuestSystem.Instance.QuestSystemRegister(quest);
        }

        QuestSystem.Instance.onQuestSystemComplete += questContent.Questcompleted;
        QuestSystem.Instance.Save();
        talkIndex = 0;
        Talking();
    }
    public void CancelQuest()
    {
        questContent.condition = TalkerConditon.Cancel;

        talkIndex = 0;
        Talking();
    }

    public void NextTalk()
    {
        Talking();
    }

    private void EndTalk()
    {
        quest = null;
        talkIndex = 0;

        this.gameObject.SetActive(false);
        Completed.GameManager.instance.ArrowKeysEnable = true;
    }

    private void InputQuest(Quest inputQuest)
    {
        this.quest = inputQuest;
    }

    private void Talking()
    {
        TalkContent content;

        if (questContent.condition == TalkerConditon.Running)
        {
            content = questContent.talks[talkIndex];
        }
        else if (questContent.condition == TalkerConditon.Acceiption)
        {
            if (talkIndex >= questContent.acceptionTalk.Count)
            {
                EndTalk();
                return;
            }
            content = questContent.acceptionTalk[talkIndex];
        }
        else
        {
            if (talkIndex >= questContent.cancelTalk.Count)
            {
                EndTalk();
                return;
            }
            content = questContent.cancelTalk[talkIndex];
        }

        ShowButton(questContent);
        Talker(content);
        talkIndex++;
    }

    private void Talker(TalkContent content)
    {
        if(content.selectTalker == SelectTalker.PLAYER)
        {
            playerPanel.SetActive(true);
            npcPanel.SetActive(false);

            playerTalkText.text = content.talkContent;
        }
        else if(content.selectTalker == SelectTalker.NPC)
        {
            playerPanel.SetActive(false);
            npcPanel.SetActive(true);

            npcTalkText.text = content.talkContent;
        }
    }

    private void ShowButton(Talker questContent)
    {
        if(talkIndex + 1 == questContent.talks.Count)
        {
            nextTalkButtons[0].gameObject.SetActive(false);
            nextTalkButtons[1].gameObject.SetActive(false);

            if(questContent.acceptionTalk.Count >= 1)  acceptionButton.gameObject.SetActive(true);
            if(questContent.cancelTalk.Count >= 1) cancelButton.gameObject.SetActive(true);
        }
        else
        {
            acceptionButton.gameObject.SetActive(false);
            cancelButton.gameObject.SetActive(false);
            nextTalkButtons[0].gameObject.SetActive(true);
            nextTalkButtons[1].gameObject.SetActive(true);
        }
    }
}
