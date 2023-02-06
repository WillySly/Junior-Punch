using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PlayerHealth : Health
{
    [SerializeField] float gameOverScreenDelay = 5f;
    [SerializeField] GameObject gameOverCanvas;

    public static event Action playerDeathEvent;
    public static event Action<int, float> playerHitEvent;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Die()
    {
        PlayerController pc = GetComponent<PlayerController>();
        pc.enabled = false;

        PlayerCombat pcombat = GetComponent<PlayerCombat>();
        pcombat.enabled = false;


        base.Die();

        if (playerDeathEvent != null)
        {
            playerDeathEvent();
        }

        StartCoroutine(PlayDyingAnimation());
    }

    public override void gotHit(int points)
    {
        base.gotHit(points);

        if (playerHitEvent != null)
        {
            playerHitEvent(points, hitAnimationDelay + healthbarAnimationDelay);
        }
    }

    IEnumerator PlayDyingAnimation()
    {
        yield return new WaitForSeconds(gameOverScreenDelay);
        gameOverCanvas.SetActive(true);
    }

}
