using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Reward/GameobjectReward", fileName = "GameobjectReward_")]
public class GameobjectReward : Reward
{
    [SerializeField]
    private GameObject value;
    public override object Value => value;

    public override void Give(Quest quest)
    {
        Debug.Log("Get GameObject Reward");
    }
}
