using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Task/Action/ContinueNegativeCount", fileName = "ContinueNegativeCount")]
public class ContinueNegativeCount : TaskAction
{
    public override int Run(Task task, int currentSuccessCount, int prevSuccessCount)
    {
        return prevSuccessCount < 0 ? currentSuccessCount - prevSuccessCount : 0;
    }
}
