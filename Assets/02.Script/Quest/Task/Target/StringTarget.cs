using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Quest/Task/Target/StringTarget", fileName ="StringTarget_")]
public class StringTarget : TaskTarget
{
    [SerializeField]
    private string value;
    public override object Value => value;

    // 같은 String인지 확인
    public override bool IsEqual(object target)
    {
        string stringTarget = target as string;

        if(stringTarget == null)
        {
            return false;
        }

        return stringTarget == value;
    }
}