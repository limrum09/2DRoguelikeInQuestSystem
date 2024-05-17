using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Quest/Task/Action/NegativeCount", fileName ="NegativeCount")]
public class NegativeCount : TaskAction
{
    public override int Run(Task task, int currentSuccessCount, int prevSuccessCount)
    {
        return prevSuccessCount < 0 ? currentSuccessCount - prevSuccessCount : prevSuccessCount;
    }
}
