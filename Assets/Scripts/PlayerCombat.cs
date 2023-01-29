using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] float attackRange = 2f;
    [SerializeField] int attackDamage = 20;
    [SerializeField] Transform attackPoint;
    [SerializeField] ParticleSystem hitEffect;
    [SerializeField] LayerMask enemyLayer;


    Animator animator;

    bool isAlive;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Space))
        {
            Attack();

        }
    }

    void Attack()
    {

        animator.SetTrigger("attack");
        Collider[] enemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);

        foreach(Collider enemy in enemies)
        {
            enemy.GetComponent<EnemyHealth>().gotHit(attackDamage);
            hitEffect.Play();

        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
