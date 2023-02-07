using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFXController : MonoBehaviour
{

    [SerializeField] AudioSource walkSound, runSound;
    [SerializeField] AudioSource deathSound, boneShutterSound;
    [SerializeField] AudioSource reachSound;
    [SerializeField] AudioSource[] hitSounds;

    [SerializeField] ParticleSystem hitEffect;
    [SerializeField] Animator animator;


    void Start()
    {
        Debug.Log(Time.time + " FXController in start");
        Debug.Log(Time.time + " animator is " + animator.isActiveAndEnabled);

        if (animator == null) Debug.Log(Time.time + " Animator is null");
        EnemyHealth.enemyDeathEvent += PlayEnemyDyingSounds;
    }


    public void Walk()
    {
        walkSound.enabled = true;
        runSound.enabled = false;

        animator.SetBool("isWalking", true);
        animator.SetBool("isIdle", false);
        animator.SetBool("isRunning", false);
    }


    public void Idle()
    {
        walkSound.enabled = false;
        runSound.enabled = false;
        animator.SetBool("isWalking", false);
        animator.SetBool("isIdle", true);
        animator.SetBool("isRunning", false);

    }

    public void Chase(bool canMove, bool reachedPlayer)
    {
        animator.SetBool("isWalking", false);
        animator.SetBool("isIdle", false);
        if (!reachedPlayer)
        {
            walkSound.enabled = false;
            if (canMove) runSound.enabled = true;
            else runSound.enabled = false;
            animator.SetBool("isRunning", true);
        }
    }


    public void Attack()
    {
        animator.SetTrigger("attack");
        animator.SetBool("isRunning", false);

        runSound.enabled = false;
        walkSound.enabled = false;
    }


    public void NotEngaged()
    {
        animator.ResetTrigger("attack");
    }

    public void Hit()
    {
        hitEffect.Play();
        int index = Random.Range(0, 2);

        if (!hitSounds[index].enabled)
        {
            hitSounds[index].enabled = true;
        }
        else
        {
            hitSounds[index].Play();
        }
    }

    public void Reach()
    {
        reachSound.enabled = true;
        if (!reachSound.enabled)
        {
            reachSound.enabled = true;
        }
        else
        {
            reachSound.Play();
        }
    }

    void PlayEnemyDyingSounds()
    {
        deathSound.enabled = true;
    }

    public void EnemyFallEvent()
    {
        boneShutterSound.enabled = true;
    }

    private void OnDestroy()
    {
        EnemyHealth.enemyDeathEvent -= PlayEnemyDyingSounds;
    }

}
