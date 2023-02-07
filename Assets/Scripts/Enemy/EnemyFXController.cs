using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyFXController : MonoBehaviour
{

    [SerializeField] AudioSource walkSound, runSound;
    [SerializeField] AudioSource deathSound, boneShutterSound;
    [SerializeField] AudioSource reachSound;
    [SerializeField] AudioSource[] hitSounds;

    [SerializeField] ParticleSystem hitEffect;
    [SerializeField] Animator animator;


    bool isHit = false; // to check which sound to play, hit or reach
    bool busy = false;
    bool engagedInCombat = false; //to play hit and attack only if engaged in combat


    void Start()
    {
        EnemyHealth.enemyDeathEvent += PlayEnemyDyingSounds;
        EnemyCombat.enemyHitEvent += Hit;
        EnemyCombat.enemyAttackEvent += Attack;
    }


    public void Walk()
    {
        walkSound.enabled = true;
        runSound.enabled = false;

        animator.SetBool("isWalking", true);
        animator.SetBool("isIdle", false);
        animator.SetBool("isRunning", false);
    }


    public void Idle()
    {
        walkSound.enabled = false;
        runSound.enabled = false;
        animator.SetBool("isWalking", false);
        animator.SetBool("isIdle", true);
        animator.SetBool("isRunning", false);
    }

    public void Chase(bool reachedPlayer)
    {
        animator.SetBool("isWalking", false);
        animator.SetBool("isIdle", false);
        if (!reachedPlayer)
        {
            walkSound.enabled = false;
            if (!busy) runSound.enabled = true;
            else runSound.enabled = false;
            animator.SetBool("isRunning", true);
        }
    }


    public void Attack()
    {
        if (engagedInCombat)
        {
            animator.SetTrigger("attack");
            animator.SetBool("isRunning", false);

            runSound.enabled = false;
            walkSound.enabled = false;
        }
    }


    void Hit(int points)
    {
        if (engagedInCombat) isHit = true;
    }

    void PlayEnemyDyingSounds()
    {

        if (engagedInCombat)
        {
            animator.SetBool("isDead", true);

            deathSound.enabled = true;
            runSound.enabled = false;

        }

    }

    private void EnemyFallEvent()
    {
        boneShutterSound.enabled = true;
    }


    public void SkeletonReachEvent()
    {
        if (isHit)
        {
            hitEffect.Play();
            int index = UnityEngine.Random.Range(0, 2);

            if (!hitSounds[index].enabled)
            {
                hitSounds[index].enabled = true;
            }
            else
            {
                hitSounds[index].Play();
            }
            isHit = false;
        }
        else
        {
            reachSound.enabled = true;
            if (!reachSound.enabled)
            {
                reachSound.enabled = true;
            }
            else
            {
                reachSound.Play();
            }
        }
    }

    public void EngageInCombat()
    {
        engagedInCombat = true;
    }

    public bool isBusy()
    {
        return busy;
    }

    private void OnDestroy()
    {
        EnemyHealth.enemyDeathEvent -= PlayEnemyDyingSounds;
        EnemyCombat.enemyHitEvent -= Hit;
        EnemyCombat.enemyAttackEvent -= Attack;
    }


    // catches attack animation ending event. Enemy must not move during attack
    private void AttackEnded()
    {
        busy = false;
    }

    private void AttackStarted()
    {
        busy = true;

    }

    private void DamageEnded()
    {
        busy = false;
    }

    private void DamageStarted()
    {
        busy = true;
    }

}