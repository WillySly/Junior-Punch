using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int enemyHealth;

    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void gotHit(int points)
    {
        enemyHealth -= points;

        animator.SetTrigger("isHurt");
        if (enemyHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("enemy died");
        animator.SetBool("isDead", true);
        GetComponent<Collider>().enabled = false;
        this.enabled = false;
    }

}
