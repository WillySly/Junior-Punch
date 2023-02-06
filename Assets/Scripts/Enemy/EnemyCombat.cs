using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    [SerializeField] int attackDamage = 20;
    [SerializeField] float attackRange = 2f;
    [SerializeField] float attackCooldownTime = 2f;
    [SerializeField] float attackAnimationDelay;

    [SerializeField] Transform attackPoint;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] ParticleSystem hitEffect;
    [SerializeField] AudioSource reachSound;
    [SerializeField] AudioSource[] hitSounds;

    enum state { attack, cooldown, notEnagged }
    bool engagedInCombat;

    float cooldown; // attack cooldown
    state currentState;

    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        engagedInCombat = false;
        cooldown = 0;
    }

    private void Update()
    {
        if (engagedInCombat)
        {
            Attack();
            engagedInCombat = false;
        }
        else
        {
            SetState(state.notEnagged);
        }
    }
    void Attack()
    {
        if (cooldown <= 0)
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

            SetState(state.attack);
            Collider[] target = Physics.OverlapSphere(attackPoint.position, attackRange, playerLayer);

            foreach (Collider player in target)
            {
                player.GetComponent<PlayerHealth>().gotHit(attackDamage);
                hitEffect.Play();
            }

            SetState(state.cooldown);
            StartCoroutine(adjustAttackToAnimation());
        }
        else
        {
            cooldown -= Time.deltaTime;
        }
    }

    void SetState(state state)
    {
        currentState = state;
        switch (state)
        {
            case state.attack:
                animator.SetTrigger("attack");
                animator.SetBool("isRunning", false);
                break;
            case state.cooldown:
                cooldown = attackCooldownTime;
                break;
            case state.notEnagged:
                animator.ResetTrigger("attack");
                cooldown = 0;
                break;
        }
    }
    IEnumerator adjustAttackToAnimation()
    {
        yield return new WaitForSeconds(attackAnimationDelay);

        int index = Random.Range(0, 2);

        if (!hitSounds[index].enabled)
        {
            hitSounds[index].enabled = true;
        }
        else
        {
            hitSounds[index].Play();
        }
    }

    public float GetAttackRange()
    {
        return attackRange;
    }

    public void EngageInCombat()
    {
        engagedInCombat = true;
    }






}
