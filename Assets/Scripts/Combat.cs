using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Combat : MonoBehaviour
{
    public event Action <int> gotHitEvent;
    public event Action hitEvent;


    public void GotHit(int attackDamage)
    {
        gotHitEvent?.Invoke(attackDamage);
    }

    public void Hit()
    {
        hitEvent?.Invoke();
    }
}
