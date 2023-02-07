using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PlayerHealth : Health
{
    [SerializeField] GameObject gameOverCanvas;

    public static event Action playerDeathEvent;
    public static event Action<int, float> playerDamageEvent;

    protected override void Start()
    {
        base.Start();

        EnemyCombat.enemyHitEvent += gotHit;

    }

    protected override void Die()
    {
        PlayerController pc = GetComponent<PlayerController>();
        pc.enabled = false;

        PlayerCombat pcombat = GetComponent<PlayerCombat>();
        pcombat.enabled = false;

        GetComponent<CapsuleCollider>().enabled = false;


        base.Die();

        if (playerDeathEvent != null)
        {
            playerDeathEvent();
        }

    }

    public override void gotHit(int points)
    {
        base.gotHit(points);

        if (playerDamageEvent != null)
        {
            playerDamageEvent(points, hitAnimationDelay + healthbarAnimationDelay);
        }
    }

    private void DyingAnimationFinished()
    {
        gameOverCanvas.SetActive(true);
    }

    public void OnDestroy()
    {
        EnemyCombat.enemyHitEvent -= gotHit;
    }

}
