using UnityEngine.AI;

public class EnemyHealth : Health
{

    protected override void Die()
    {
        base.Die();
        GetComponent<EnemyAI>().enabled = false;
        GetComponent<NavMeshAgent>().enabled = false;
        if (healthbar != null)
        {
            healthbar.gameObject.SetActive(false);
        }
    }

}
