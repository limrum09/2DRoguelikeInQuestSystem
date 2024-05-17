using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Quest/Task/Action/PositiveCount", fileName ="PositiveCount")]
public class PositiveCount : TaskAction
{
    public override int Run(Task task, int currentSuccessCount, int prevSuccessCount)
    {
        return prevSuccessCount > 0 ? currentSuccessCount + prevSuccessCount : prevSuccessCount;
    }
}