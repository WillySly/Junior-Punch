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


    Animator animator;


    void Start()
    {
        animator = GetComponent<Animator>();

        PlayerHealth.playerDeathEvent += PlayDyingSounds;
    }

    public void walk()
    {
        animator.SetBool("isMoving", true);
        walkSound.enabled = true;
    }

    public void run()
    {
        animator.SetBool("isRunning", true);
        walkSound.enabled = false;
        runSound.enabled = true;
    }

    public void stopMoving()
    {
        animator.SetBool("isMoving", false);
        walkSound.enabled = false;
    }

    public void stopRunning()
    {
        animator.SetBool("isRunning", false);
        runSound.enabled = false;
    }

    public void kick()
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

    public void hit()
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
    public void attack()
    {
        animator.SetTrigger("attack");
    }

    void PlayDyingSounds()
    {
        deathSound.enabled = true;
    }
}
