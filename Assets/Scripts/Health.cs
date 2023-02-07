using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;


// Parent class for all things health related

public class Health : MonoBehaviour
{
    [SerializeField] protected int health;
    [SerializeField] protected float hitAnimationDelay = 0.3f;
    [SerializeField] protected float healthbarAnimationDelay = 0.1f;
    [SerializeField] protected float healthbarUpdateSpeedSeconds = 0.5f;
    [SerializeField] protected Image healthbarForegroundImage;

    protected Transform healthbar;
    protected Animator animator;
    protected int initHealth;

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        initHealth = health;
        healthbar = transform.Find("Healthbar");
    }

    protected virtual void Die()
    {
        animator.SetBool("isDead", true);
        GetComponent<Collider>().enabled = false;
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
        animator.ResetTrigger("isHurt");
    }

    public virtual void gotHit(int points)
    {
        health -= points;

        // Animations are not synced with logic, so we make them wait
        StartCoroutine(playHitAnimation());

        if (health <= 0)
        {
            Die();
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
        StartCoroutine(updateHealthbar());
    }


    void LateUpdate()
    {
        healthbar.LookAt(Camera.main.transform);
    }



}
