using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerCombat : Combat
{
    [SerializeField] float attackRange = 2f;
    [SerializeField] int attackDamage = 20;
    [SerializeField] Transform attackPoint;
    [SerializeField] LayerMask enemyLayer;

    public static event Action playerKickEvent;
    public static event Action playerAttackEvent;

    float attackPointRadius = 5f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
    }

    void Attack()
    {
        playerAttackEvent?.Invoke();
    }

    public void PlayerHitEvent()
    {
        Collider[] enemies = Physics.OverlapSphere(attackPoint.position, attackPointRadius, enemyLayer);

        foreach (Collider enemy in enemies)
        {

            Hit();
            enemy.gameObject.GetComponent<EnemyCombat>().GotHit(attackDamage);

        }

        playerKickEvent?.Invoke();
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
