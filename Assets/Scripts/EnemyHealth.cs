using UnityEngine.AI;
using System.Collections;
using UnityEngine;



public class EnemyHealth : Health
{

    [SerializeField] float dieAnimationDelay = 1f;
    [SerializeField] float disappearDelay = 3f;

    protected override void Die()
    {
        base.Die();
        GetComponent<EnemyAI>().enabled = false;
        GetComponent<NavMeshAgent>().enabled = false;

        StartCoroutine(playDyingAnimation());

    }


    public IEnumerator playDyingAnimation()
    {
        yield return new WaitForSeconds(dieAnimationDelay);
        if (healthbar != null)
        {
            healthbar.gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(disappearDelay);
        GameObject.Destroy(gameObject);


    }

}
