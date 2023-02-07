using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] float attackRange = 2f;
    [SerializeField] int attackDamage = 20;
    [SerializeField] Transform attackPoint;
    [SerializeField] LayerMask enemyLayer;

    public static event Action playerHitEvent;
    public static event Action playerKickEvent;
    public static event Action playerAttackEvent;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
    }

    void Attack()
    {
        if (playerAttackEvent != null)
        {
            playerAttackEvent();
        }

        Collider[] enemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);

        foreach (Collider enemy in enemies)
        {
            enemy.GetComponent<Health>().gotHit(attackDamage);
            if (playerHitEvent != null)
            {
                playerHitEvent();
            }
        }

        if (playerKickEvent != null)
        {
            playerKickEvent();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
