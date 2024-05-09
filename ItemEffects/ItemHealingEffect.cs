using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEffect/Consumable/HP")]
public class ItemHealingEffect : ItemEffect
{
    public int healingPoint;

    public override bool ExecuteRole()
    {
        GameManager.instance.player.hp += healingPoint;
        return true;
    }
}
