using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SelectTalker
{
    NPC,
    PLAYER
}

[CreateAssetMenu(menuName ="Talk/TalkContent", fileName ="TalkContent_")]
public class TalkContent : ScriptableObject
{
    [SerializeField]
    public SelectTalker selectTalker;
    [SerializeField]
    public string talkContent;
}
