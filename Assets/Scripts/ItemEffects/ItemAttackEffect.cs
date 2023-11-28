using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "ItemEffect/Consumable/Attack")]
public class ItemAttackEffect : ItemEffect
{
    public float attackPoint = 2;

    public override bool ExecuteRole()
    {
        GameManager.instance.player.attack += 2;
        GameManager.instance.atkPotionOn.color = new Color32(255, 255, 255, 255);
        GameManager.instance.player.atkPotion = true;
        return true;
    }
}