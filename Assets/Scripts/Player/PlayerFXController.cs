using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFXController : MonoBehaviour
{
    [SerializeField] public AudioSource walkSound, runSound, deathSound;
    [SerializeField] AudioSource kickSound;

    // Hit sounds to choose randomly from during combat
    [SerializeField] AudioSource[] hitSounds;

    [SerializeField] ParticleSystem hitEffect;
    [SerializeField] Animator animator;

    bool isHit = false;

    void Start()
    {
        PlayerHealth.playerDeathEvent += PlayDyingSounds;
        PlayerCombat.playerAttackEvent += Attack;
        PlayerCombat.playerHitEvent += Hit;
        PlayerCombat.playerKickEvent += Kick;

    }

    public void Walk()
    {
        animator.SetBool("isMoving", true);
        walkSound.enabled = true;
    }

    public void Run()
    {
        animator.SetBool("isRunning", true);
        walkSound.enabled = false;
        runSound.enabled = true;
    }

    public void StopMoving()
    {
        animator.SetBool("isMoving", false);
        walkSound.enabled = false;
    }

    public void StopRunning()
    {
        animator.SetBool("isRunning", false);
        runSound.enabled = false;
    }

    public void Hit()
    {
        isHit = true;
    }

    public void Kick()
    {
        if (isHit)
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
            isHit = false;
        }
        else
        {
            kickSound.enabled = true;
            if (!kickSound.enabled)
            {
                kickSound.enabled = true;
            }
            else
            {
                kickSound.Play();
            }
        }
    }

    public void Attack()
    {
        animator.SetTrigger("attack");
    }

    void PlayDyingSounds()
    {
        animator.SetBool("isDead", true);

        deathSound.enabled = true;
    }

    private void OnDestroy()
    {
        PlayerHealth.playerDeathEvent -= PlayDyingSounds;
        PlayerCombat.playerAttackEvent -= Attack;
        PlayerCombat.playerHitEvent -= Hit;
        PlayerCombat.playerKickEvent -= Kick;
    }

}
