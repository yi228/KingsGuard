using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastScene : MonoBehaviour
{
    public BigBoss bigBoss;

    void Update()
    {
        if(GameManager.instance.kingDead)
            bigBoss.gameObject.SetActive(true);
        else
            bigBoss.gameObject.SetActive(false);
    }
}
