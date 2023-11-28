using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Creature : MonoBehaviour
{
    public float hp;
    public float maxhp;
    public float attack;
    public float speed;

    public abstract void Attack();
    public abstract void Move();
    public abstract void GetHit();
    public abstract void Death();
}
