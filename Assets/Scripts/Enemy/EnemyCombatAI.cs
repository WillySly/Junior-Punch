using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyCombatAI : Combat
{
    [SerializeField] int attackDamage = 20;
    [SerializeField] float attackRange = 2f;
    [SerializeField] float attackCooldownTime = 2f;

    [SerializeField] Transform attackPoint;
    [SerializeField] LayerMask playerLayer;

    public static event Action enemyAttackEvent;
    public static event Action strikeEvent;


    enum state { attack, cooldown, notEnagged }
    bool engagedInCombat;
    state currentState;

    float cooldown; // attack cooldown
    float attackPointRadius = 5f;

    private void Awake()
    {
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
            SetState(state.attack);

        }
        else
        {
            cooldown -= Time.deltaTime;
        }
    }

    void SetState(state state)
    {
        if (currentState != state)
        {
            currentState = state;
            switch (state)
            {
                case state.attack:
                    enemyAttackEvent?.Invoke();

                    break;
                case state.cooldown:
                    cooldown = attackCooldownTime;
                    break;
                case state.notEnagged:
                    cooldown = 0;
                    break;
            }
        }
       
       
    }

    public void SkeletonReachEvent()
    {
        
        Collider[] target = Physics.OverlapSphere(attackPoint.position, attackPointRadius, playerLayer);

        foreach (Collider player in target)
        {
            Hit();
            player.gameObject.GetComponent<PlayerCombat>().GotHit(attackDamage);
        }

        strikeEvent?.Invoke();
        SetState(state.cooldown);
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
