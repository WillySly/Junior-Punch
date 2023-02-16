using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PlayerHealth : Health
{
    private void OnEnable()
    {
        GetComponent<Combat>().gotHitEvent += gotHit;
    }

    protected override void gotHit(int points)
    {
        base.gotHit(points);
    }

    private void OnDisable()
    {
        GetComponent<Combat>().gotHitEvent -= gotHit;

    }

}
