using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : Health
{
    [SerializeField] float dieAnimationDelay = 5f;

    public override void gotHit(int points)
    {
        
        base.gotHit(points);

    }

    public IEnumerator playHitAnimation()
    {
        yield return new WaitForSeconds(hitAnimationDelay);
    }

    protected override void Die()
    {
        PlayerController pc = GetComponent<PlayerController>();
        pc.enabled = false;
        PlayerCombat pcombat = GetComponent<PlayerCombat>();
        pcombat.enabled = false;
        base.Die();

        StartCoroutine(playDyingAnimation());

 
    }


    public IEnumerator playDyingAnimation()
    {
        yield return new WaitForSeconds(dieAnimationDelay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
