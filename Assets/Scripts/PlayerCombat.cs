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
    [SerializeField] AudioSource kickSound;
    [SerializeField] AudioSource[] hitSounds;


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
            kickSound.enabled = true;
            if (!kickSound.enabled)
            {
                kickSound.enabled = true;
            }else
            {
                kickSound.Play();
            }
            Attack();

        }
    }

    void Attack()
    {

        animator.SetTrigger("attack");
        Collider[] enemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);

        foreach(Collider enemy in enemies)
        {
            enemy.GetComponent<Health>().gotHit(attackDamage);
            hitEffect.Play();

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
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
