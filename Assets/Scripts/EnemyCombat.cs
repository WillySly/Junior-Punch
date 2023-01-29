using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    [SerializeField] float attackRange = 2f;
    [SerializeField] int attackDamage = 20;
    [SerializeField] float attackCooldownTime = 2f;
    [SerializeField] Transform attackPoint;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] ParticleSystem hitEffect;
    [SerializeField] float attackAnimationDelay;

    enum state { attack, cooldown, notEnagged }
    bool engagedInCombat;

    float cooldown;
    state currentState;


    EnemyAI ai;
    EnemyHealth health;
    Animator animator;

    private void Awake()
    {
        ai = GetComponent<EnemyAI>();
        health = GetComponent<EnemyHealth>();
        animator = GetComponent<Animator>();

        engagedInCombat = false;
        cooldown = 0;
    }

    private void Update()
    {

        //Debug.Log(Time.time + " EnemyCombat: currentState " + currentState);
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

    void SetState(state state)
    {
        currentState = state;
        switch (state)
        {
            case state.attack:
                //Debug.Log("setting to attack");
                animator.SetTrigger("attack");

                animator.SetBool("isRunning", false);

                break;
            case state.cooldown:
                //Debug.Log("setting to cooldown");
                cooldown = attackCooldownTime;

                break;
            case state.notEnagged:
                animator.ResetTrigger("attack");
                cooldown = 0;
                break;
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

    void Attack()
    {
        if (cooldown <= 0)
        {
            SetState(state.attack);
            Collider[] target = Physics.OverlapSphere(attackPoint.position, attackRange, playerLayer);

            foreach (Collider player in target)
            {
                player.GetComponent<PlayerHealth>().gotHit(attackDamage);
                hitEffect.Play();
                //Debug.Log("hiteffect: " + hitEffect.gameObject.name + "  is playing " + hitEffect.isPlaying);

            }

            SetState(state.cooldown);

            //StartCoroutine(adjustAttackToAnimation());
            //Debug.Log("attacking");

          
        }
        else
        {
            cooldown -= Time.deltaTime;
        }
    }

    //public IEnumerator adjustAttackToAnimation()
    //{
    //    yield return new WaitForSeconds(attackAnimationDelay);
       
    //}




}
