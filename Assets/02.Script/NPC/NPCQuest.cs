using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCQuest : MonoBehaviour
{
    [SerializeField]
    public Talker questContent;
    [SerializeField]
    private Quest quest;

    private int questIndex;
    private bool isReceiveQuest;
    public bool npcGiveQuestToPlayer;

    private void Start()
    {
        npcGiveQuestToPlayer = false;
        isReceiveQuest = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(isReceiveQuest == true)
            {
                UIPanelController.Instance.ShowTalkPanel(quest, questContent);
                npcGiveQuestToPlayer = true;
                isReceiveQuest = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collider!");
        if (collision.collider.CompareTag("Player"))
        {
            Debug.Log("Collider Player!");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (questContent.condition != TalkerConditon.Acceiption && quest.IsAcceiption && !npcGiveQuestToPlayer)
                isReceiveQuest = true;
            else
                isReceiveQuest = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isReceiveQuest = false;
        }
    }
}
