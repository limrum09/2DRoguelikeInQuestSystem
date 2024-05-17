using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TaskTarget : ScriptableObject
{
    // Target값 출력
    public abstract object Value { get; }

    // Value와 확인할 target의 값이 같은지 확인
    public abstract bool IsEqual(object target);
}
