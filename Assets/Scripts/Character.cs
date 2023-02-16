using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Character : MonoBehaviour
{
    [SerializeField] float disappearDelay = 5f;

    private void OnEnable()
    {
        GetComponent<Health>().deathEvent += Die;
    }
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
