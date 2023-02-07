using UnityEngine.AI;
using System.Collections;
using UnityEngine;
using System;


public class EnemyHealth : Health
{
    [SerializeField] float disappearDelay = 5f;

    public static event Action enemyDeathEvent;
   
    protected override void Die()
    {
        base.Die();
        GetComponent<EnemyAI>().enabled = false;
        GetComponent<NavMeshAgent>().enabled = false;
        GetComponent<CapsuleCollider>().enabled = false;

        StartCoroutine(Disappear());

        if (enemyDeathEvent != null)
        {
            enemyDeathEvent();
        }
    }


    private void EnemyFallEvent()
    {
        if (healthbar != null)
        {
            healthbar.gameObject.SetActive(false);
        }
    }

    IEnumerator Disappear()
    {
        yield return new WaitForSeconds(disappearDelay);
        Destroy(gameObject);
    }


}
