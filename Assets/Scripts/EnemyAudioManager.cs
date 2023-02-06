using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudioManager : MonoBehaviour
{
    [SerializeField] AudioSource deathSound, boneShutterSound;
    void Start()
    {
        EnemyHealth.enemyDeathEvent += PlayEnemyDyingSounds;
    }

    void PlayEnemyDyingSounds()
    {
        deathSound.enabled = true;
    }


    // Middle of dying animation
    public void EnemyFallEvent()
    {
        boneShutterSound.enabled = true;
    }

    private void OnDestroy()
    {
        EnemyHealth.enemyDeathEvent -= PlayEnemyDyingSounds;
    }
}
