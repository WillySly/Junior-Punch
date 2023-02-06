using UnityEngine.AI;
using System.Collections;
using UnityEngine;
using TMPro;
using System;


public class EnemyHealth : Health
{
    [SerializeField] float dieAnimationDelay = 1f;
    [SerializeField] float disappearDelay = 3f;

    public static event Action enemyDeathEvent;
   
    protected override void Die()
    {
        base.Die();
        GetComponent<EnemyAI>().enabled = false;
        GetComponent<NavMeshAgent>().enabled = false;
        StartCoroutine(PlayDyingAnimation());

        if (enemyDeathEvent != null)
        {
            enemyDeathEvent();
        }
    }

    IEnumerator PlayDyingAnimation()
    {
        yield return new WaitForSeconds(dieAnimationDelay);

        if (healthbar != null)
        {
            healthbar.gameObject.SetActive(false);
        }


        yield return new WaitForSeconds(disappearDelay);
        Destroy(gameObject);
    }


}
