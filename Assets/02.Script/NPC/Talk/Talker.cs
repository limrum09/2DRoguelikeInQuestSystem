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

    private void OnDestroy()
    {
        QuestSystem.Instance.onQuestSystemComplete -= Questcompleted;
    }

    public void Questcompleted()
    {
        Debug.Log("����Ʈ �ذ�!");
        condition = TalkerConditon.Running;
    }
}
