using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Character : MonoBehaviour
{
    [SerializeField] float disappearDelay = 5f;


    protected virtual void Die()
    {
        Debug.Log("die");
        GetComponent<Collider>().enabled = false;
    }

    protected IEnumerator Disappear()
    {
        
        yield return new WaitForSeconds(disappearDelay);
        Debug.Log("gonna destriy now");
        Destroy(gameObject);

    }

    private void OnDisable()
    {
        GetComponent<Health>().deathEvent -= Die;
    }



}
