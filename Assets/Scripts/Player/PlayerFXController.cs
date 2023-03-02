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

    bool strikeHit = false;

    void OnEnable()
    {
        GetComponent<Health>().deathEvent += PlayDyingSounds;
        PlayerCombat.playerAttackEvent += Attack;
        GetComponent<Combat>().hitEvent += Strike;
        PlayerCombat.playerKickEvent += Kick;
        GetComponent<Combat>().gotHitEvent += GotHit;

        GetComponent<PlayerController>().walk += Walk;
        GetComponent<PlayerController>().run += Run;
        GetComponent<PlayerController>().stopMoving += StopMoving;
        GetComponent<PlayerController>().stopRunning += StopRunning;

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

    public void Strike()
    {
        strikeHit = true;
    }

    public void Kick()
    {
        if (strikeHit)
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
            strikeHit = false;
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

    void GotHit(int points)
    {
        animator.SetTrigger("isHurt");
    }

    void PlayDyingSounds()
    {
        animator.SetBool("isDead", true);

        deathSound.enabled = true;
    }

    private void OnDisable()
    {
        GetComponent<Health>().deathEvent -= PlayDyingSounds;
        PlayerCombat.playerAttackEvent -= Attack;
        GetComponent<Combat>().hitEvent -= Strike;
        PlayerCombat.playerKickEvent -= Kick;
        GetComponent<Combat>().gotHitEvent -= GotHit;

        GetComponent<PlayerController>().walk -= Walk;
        GetComponent<PlayerController>().run -= Run;
        GetComponent<PlayerController>().stopMoving -= StopMoving;
        GetComponent<PlayerController>().stopRunning -= StopRunning;
    }

}
