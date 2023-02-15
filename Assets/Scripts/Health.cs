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

    public static event Action<GameObject> deathEvent;
    public static event Action<int, GameObject> updateHealthEvent;


    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
    }



    public virtual void gotHit(int points)
    {
        health -= points;
        Debug.Log("gotHit");

        // Animations are not synced with logic, so we make them wait
        StartCoroutine(playHitAnimation());

        if (health <= 0)
        {
            deathEvent(gameObject);
        }
    }

    public int GetHealth()
    {
        return health;
    }


    IEnumerator playHitAnimation()
    {
        yield return new WaitForSeconds(hitAnimationDelay);
        animator.SetTrigger("isHurt");
        yield return new WaitForSeconds(healthbarAnimationDelay);
        updateHealthEvent(health, gameObject);

    }






}
