using UnityEngine.AI;
using System.Collections;
using UnityEngine;
using System;


public class EnemyHealth : Health
{

    private void EnemyFallEvent()
    {
        if (healthbar != null)
        {
            healthbar.gameObject.SetActive(false);
        }
    }




}
