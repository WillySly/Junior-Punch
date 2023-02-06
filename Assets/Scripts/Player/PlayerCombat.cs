using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] float attackRange = 2f;
    [SerializeField] int attackDamage = 20;
    [SerializeField] Transform attackPoint;
    [SerializeField] LayerMask enemyLayer;

    PlayerFXController FXController;

    void Start()
    {
        FXController = GetComponent<PlayerFXController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Space))
        {
            FXController.kick();
            Attack();
        }
    }

    void Attack()
    {
        FXController.attack();
     
        Collider[] enemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);

        foreach(Collider enemy in enemies)
        {
            enemy.GetComponent<Health>().gotHit(attackDamage);
            FXController.hit();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
