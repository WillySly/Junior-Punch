using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;


// Parent class for all things health related

public class Health : MonoBehaviour
{
    [SerializeField] public int health;
    [SerializeField] protected float hitAnimationDelay = 0.3f;

    protected float healthbarAnimationDelay = 0.1f;
    protected Animator animator;

    public event Action deathEvent;
    public static event Action deathStaticEvent;
    public event Action<int> updateHealthEvent;

    private void OnEnable()
    {
        GetComponent<Combat>().gotHitEvent += gotHit;
    }

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
    }

    protected virtual void gotHit(int points)
    {
        health -= points;

        updateHealthEvent?.Invoke(health);

        if (health <= 0)
        {
            deathEvent?.Invoke();
            deathStaticEvent?.Invoke();
        }
    }

    public int GetHealth()
    {
        return health;
    }

    private void OnDisable()
    {
        GetComponent<Combat>().gotHitEvent -= gotHit;
    }


}
