using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] float disappearDelay = 5f;

    private void OnEnable()
    {
        Health.deathEvent += Die;
    }

    private void Start()
    {
       

    }


    protected virtual void Die(GameObject gameObject)
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
        Health.deathEvent -= Die;
    }



}
