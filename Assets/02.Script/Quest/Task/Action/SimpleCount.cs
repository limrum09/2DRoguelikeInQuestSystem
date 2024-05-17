using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Task/Action/SimpleCount", fileName = "SimpleCount")]
public class SimpleCount : TaskAction
{
    public override int Run(Task task, int currentSuccessCount, int prevSuccessCount)
    {
        return currentSuccessCount + prevSuccessCount;
    }
}
