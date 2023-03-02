using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthbarUI : HealthbarUI
{
    // Start is called before the first frame update
    protected override void OnEnable()
    {
        base.OnEnable();
        character.GetComponent<EnemyAnimationsAndFXController>().enemyFallEvent += EnemyFallEvent;
    }

    private void EnemyFallEvent()
    {
        if (transform != null)
        {
            transform.gameObject.SetActive(false);
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        character.GetComponent<EnemyAnimationsAndFXController>().enemyFallEvent -= EnemyFallEvent;
    }
}
