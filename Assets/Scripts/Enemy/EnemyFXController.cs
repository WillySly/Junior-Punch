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


    bool strikeHit = false; // to check which sound to play, hit or reach
    bool busy = false;
    bool engagedInCombat = false; //to play hit and attack only if engaged in combat

    public event Action enemyFallEvent;
    public event Action<bool> isBusy;



    void OnEnable()
    {
        GetComponent<Health>().deathEvent += PlayEnemyDyingSounds;
        GetComponent<Combat>().hitEvent += Hit;
        EnemyCombat.enemyAttackEvent += Attack;
        EnemyCombat.strikeEvent += ReachHand;
        GetComponent<Combat>().gotHitEvent += GotHit;
        GetComponent<EnemyAI>().walk += Walk;
        GetComponent<EnemyAI>().idle += Idle;
        GetComponent<EnemyAI>().chase += Chase;
        GetComponent<EnemyAI>().engageInCombat += EngageInCombat;

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

    private void GotHit(int points)
    {
        animator.SetTrigger("isHurt");

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


    void Hit()
    {
        if (engagedInCombat) strikeHit = true;
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
        enemyFallEvent();
    }


    public void ReachHand()
    {
        if (strikeHit)
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
            strikeHit = false;
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


    private void OnDisable()
    {
        GetComponent<Health>().deathEvent -= PlayEnemyDyingSounds;
        GetComponent<Combat>().hitEvent -= Hit;
        EnemyCombat.enemyAttackEvent -= Attack;
        EnemyCombat.strikeEvent -= ReachHand;
        GetComponent<Combat>().gotHitEvent -= GotHit;
        GetComponent<EnemyAI>().walk -= Walk;
        GetComponent<EnemyAI>().idle -= Idle;
        GetComponent<EnemyAI>().chase -= Chase;
        GetComponent<EnemyAI>().engageInCombat -= EngageInCombat;

    }


    // catches attack animation ending event. Enemy must not move during attack
    private void AttackEnded()
    {
        busy = false;
        isBusy?.Invoke(busy);
    }

    private void AttackStarted()
    {
        busy = true;
        isBusy?.Invoke(busy);


    }

    private void DamageEnded()
    {
        busy = false;
        isBusy?.Invoke(busy);

    }

    private void DamageStarted()
    {
        busy = true;
        isBusy?.Invoke(busy);

    }


}