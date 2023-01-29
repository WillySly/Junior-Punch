using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] protected int health;
    [SerializeField] protected float healthbarAnimationDelay = 0.1f;
    [SerializeField] protected float healthbarUpdateSpeedSeconds = 0.5f;
    [SerializeField] protected Image healthbarForegroundImage;

    protected Animator animator;
    protected int initHealth;

    void Start()
    {
        animator = GetComponent<Animator>();
        initHealth = health;
    }

    public virtual void gotHit(int points)
    {
        health -= points;
        StartCoroutine(updateHealthbar());

        animator.SetTrigger("isHurt");
        if (health <= 0)
        {
            Die();
        }
    }

    protected void Die()
    {   
        animator.SetBool("isDead", true);
        GetComponent<Collider>().enabled = false;
        this.enabled = false;
    }

    protected IEnumerator updateHealthbar()
    {
        float pct = (float)health / (float)initHealth;
        float currHealth = healthbarForegroundImage.fillAmount;
        float elapsed = 0f;

        while (elapsed < healthbarUpdateSpeedSeconds)
        {
            elapsed += Time.deltaTime;
            healthbarForegroundImage.fillAmount = Mathf.Lerp(currHealth, pct, elapsed / healthbarUpdateSpeedSeconds);
            yield return null;
        }

        healthbarForegroundImage.fillAmount = pct;
    }

}
