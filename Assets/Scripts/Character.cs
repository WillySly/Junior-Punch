using System.Collections;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] float disappearDelay = 5f;


    protected virtual void Die()
    {
        GetComponent<Collider>().enabled = false;
    }

    protected IEnumerator Disappear()
    {
        yield return new WaitForSeconds(disappearDelay);
        Destroy(gameObject);
    }

    private void OnDisable()
    {
        GetComponent<Health>().deathEvent -= Die;
    }



}
