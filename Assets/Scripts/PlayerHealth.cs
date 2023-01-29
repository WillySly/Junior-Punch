using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int health;
    [SerializeField] float animationDelay;

    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void gotHit(int points)
    {
        health -= points;

        StartCoroutine(playAnimation());
        
        if (health <= 0)
        {
            Die();
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
        yield return new WaitForSeconds(animationDelay);
        animator.SetTrigger("isHurt");

        // Insert your Play Animations here

    }

}
