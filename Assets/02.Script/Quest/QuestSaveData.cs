using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct QuestSaveData
{
    public string codeName;
    public QuestState state;
    public int taskGroupIndex;
    public int[] taskSuccessCount;
}
