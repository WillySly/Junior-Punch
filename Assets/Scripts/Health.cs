using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;


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

    void Start()
    {
        animator = GetComponent<Animator>();
        initHealth = health;
        healthbar = transform.Find("Healthbar");
    }

    public virtual void gotHit(int points)
    {
        health -= points;

        StartCoroutine(playHitAnimation());

        if (health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {   
        animator.SetBool("isDead", true);
        GetComponent<Collider>().enabled = false;

        Transform healthbar = transform.Find("Healthbar");
 
    }

    public IEnumerator playHitAnimation()
    {
        yield return new WaitForSeconds(hitAnimationDelay);
        animator.SetTrigger("isHurt");
        yield return new WaitForSeconds(healthbarAnimationDelay);
        StartCoroutine(updateHealthbar());
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

    private void LateUpdate()
    {
        healthbar.LookAt(Camera.main.transform);
    }

}
