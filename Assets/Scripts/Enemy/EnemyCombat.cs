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


    enum state { attack, cooldown, notEnagged }
    bool engagedInCombat;

    float cooldown; // attack cooldown
    state currentState;

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

            FXController.Reach();
            SetState(state.attack);
            Collider[] target = Physics.OverlapSphere(attackPoint.position, attackRange, playerLayer);

            foreach (Collider player in target)
            {
                player.GetComponent<PlayerHealth>().gotHit(attackDamage);
                FXController.Hit();
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
                FXController.Attack();
                break;
            case state.cooldown:
                cooldown = attackCooldownTime;
                break;
            case state.notEnagged:
                FXController.NotEngaged();
                cooldown = 0;
                break;
        }
    }
    IEnumerator adjustAttackToAnimation()
    {
        yield return new WaitForSeconds(attackAnimationDelay);

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
