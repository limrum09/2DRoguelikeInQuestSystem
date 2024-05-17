using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Reward : ScriptableObject
{
    [SerializeField]
    private string description;
    [SerializeField]
    private Sprite icon;

    public abstract object Value { get; }

    public string Description => description;
    public Sprite Icon => icon;

    public abstract void Give(Quest quest);
}
