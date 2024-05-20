using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TalkerConditon
{
    Running,
    Acceiption,
    Cancel
}

[CreateAssetMenu(menuName ="Talk/Talker", fileName ="Talker_")]
public class Talker : ScriptableObject
{
    [SerializeField]
    public TalkerConditon condition;
    [SerializeField]
    public string npcName;
    [SerializeField]
    public Sprite npcImage;
    [SerializeField]
    public List<TalkContent> talks;
    [SerializeField]
    public List<TalkContent> acceptionTalk;
    [SerializeField]
    public List<TalkContent> cancelTalk;
    
    public void Questcompleted()
    {
        Debug.Log("Äù½ºÆ® ÇØ°á!");
        condition = TalkerConditon.Running;

        QuestSystem.Instance.onQuestSystemComplete -= Questcompleted;
    }
}
