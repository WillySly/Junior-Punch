using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PlayerHealth : Health
{


    public static event Action<int, float> playerDamageEvent;

    private void OnEnable()
    {
        EnemyCombat.enemyHitEvent += gotHit;
    }



    public override void gotHit(int points)
    {
        base.gotHit(points);

        if (playerDamageEvent != null)
        {
            playerDamageEvent(points, hitAnimationDelay + healthbarAnimationDelay);
        }
    }




    private void OnDisable()
    {
        EnemyCombat.enemyHitEvent -= gotHit;

    }

}
