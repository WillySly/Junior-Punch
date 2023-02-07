using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyCombat : MonoBehaviour
{
    [SerializeField] int attackDamage = 20;
    [SerializeField] float attackRange = 2f;
    [SerializeField] float attackCooldownTime = 2f;
    [SerializeField] float attackAnimationDelay;

    [SerializeField] Transform attackPoint;
    [SerializeField] LayerMask playerLayer;

    public static event Action<int> enemyHitEvent;
    public static event Action enemyAttackEvent;


    enum state { attack, cooldown, notEnagged }
    bool engagedInCombat;

    float cooldown; // attack cooldown

    EnemyFXController FXController;
    private void Awake()
    {
        FXController = GetComponent<EnemyFXController>();
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
            Collider[] target = Physics.OverlapSphere(attackPoint.position, attackRange, playerLayer);

            foreach (Collider player in target)
            {
                if (enemyHitEvent != null)
                {
                    enemyHitEvent(attackDamage);
                }
            }

            SetState(state.cooldown);
        }
        else
        {
            cooldown -= Time.deltaTime;
        }
    }

    void SetState(state state)
    {
        switch (state)
        {
            case state.attack:
                if (enemyAttackEvent != null)
                {
                    enemyAttackEvent();
                }
                break;
            case state.cooldown:
                cooldown = attackCooldownTime;
                break;
            case state.notEnagged:
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






}
