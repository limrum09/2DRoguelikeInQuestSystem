using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Completed;

[CreateAssetMenu(menuName = "Reward/PointReward", fileName = "PointReward_")]
public class PointReward : Reward
{
    [SerializeField]
    private int value;

    public override object Value => value;

    public override void Give(Quest quest)
    {
        Completed.ExternalCallPlayer.Instance.GetReward(value);
    }
}
