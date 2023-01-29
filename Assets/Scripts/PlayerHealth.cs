using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int health;
    [SerializeField] float hitAnimationDelay = 0.3f;
    [SerializeField] float healthbarAnimationDelay = 0.1f;

    [SerializeField] Image foregroundImage;
    [SerializeField] float updateSpeedSeconds = 0.5f;

    Animator animator;
    int initHealth;

    void Start()
    {
        animator = GetComponent<Animator>();
        initHealth = health;
    }

    public void gotHit(int points)
    {
        //Debug.Log(Time.time + "Got hit");
        health -= points;
         
        StartCoroutine(playAnimation());
        
        if (health <= 0)
        {
            //Die();
        }
    }

    void Die()
    {
        //Debug.Log("you died");
        animator.SetBool("isDead", true);
        //GetComponent<Collider>().enabled = false;
        //this.enabled = false;

        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public IEnumerator playAnimation()
    {
        yield return new WaitForSeconds(hitAnimationDelay);
        animator.SetTrigger("isHurt");
        yield return new WaitForSeconds(healthbarAnimationDelay);
        StartCoroutine(updateHealthbar());

        // Insert your Play Animations here

    }

    public IEnumerator updateHealthbar()
    {
        float pct = (float)health / (float)initHealth;
        float currHealth = foregroundImage.fillAmount;
        float elapsed = 0f;

        while (elapsed < updateSpeedSeconds)
        {
            elapsed += Time.deltaTime;
            foregroundImage.fillAmount = Mathf.Lerp(currHealth, pct, elapsed/updateSpeedSeconds);
            yield return null;
        }

        foregroundImage.fillAmount = pct;
        

    }

}
