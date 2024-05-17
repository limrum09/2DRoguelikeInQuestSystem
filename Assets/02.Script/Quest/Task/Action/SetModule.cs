using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Quest/Task/Action/SetModule", fileName ="SetModule")]
public class SetModule : TaskAction
{
    public override int Run(Task task, int currentSuccessCount, int prevSuccessCount)
    {
        return currentSuccessCount;
    }
}
