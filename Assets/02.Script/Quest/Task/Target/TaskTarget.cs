using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TaskTarget : ScriptableObject
{
    // Target�� ���
    public abstract object Value { get; }

    // Value�� Ȯ���� target�� ���� ������ Ȯ��
    public abstract bool IsEqual(object target);
}
