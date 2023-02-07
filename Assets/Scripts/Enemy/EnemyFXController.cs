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

    public static event Action<bool> EnemyAnimationFinishedEvent;


    bool isHit = false; // to check which sound to playe, hit or reach
    bool busy = true;


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
        animator.SetTrigger("attack");
        animator.SetBool("isRunning", false);

        runSound.enabled = false;
        walkSound.enabled = false;
    }


    void Hit(int points)
    {
        isHit = true;
    }

    void PlayEnemyDyingSounds()
    {
        deathSound.enabled = true;
    }

    public void EnemyFallEvent()
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

    private void OnDestroy()
    {
        EnemyHealth.enemyDeathEvent -= PlayEnemyDyingSounds;
        EnemyCombat.enemyHitEvent -= Hit;
        EnemyCombat.enemyAttackEvent -= Attack;
    }


    // catches attack animation ending event. Enemy must not move during attack
    public void AttackEnded()
    {
        busy = false;
        EnemyAnimationFinishedEvent(busy);
    }

    public void AttackStarted()
    {
        busy = true;
        EnemyAnimationFinishedEvent(busy);

    }

    public void DamageEnded()
    {
        busy = false;
        EnemyAnimationFinishedEvent(busy);

    }

    public void DamageStarted()
    {
        busy = true;
        EnemyAnimationFinishedEvent(busy);
    }
}
