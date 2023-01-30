using UnityEngine.AI;
using System.Collections;
using UnityEngine;
using TMPro;


public class EnemyHealth : Health
{

    [SerializeField] float dieAnimationDelay = 1f;
    [SerializeField] float disappearDelay = 3f;
    [SerializeField] AudioSource deathSound, boneShattersound;
    
    RectTransform scoreUi;
    TMP_Text scoreText;

    protected override void Die()
    {
        base.Die();
        GetComponent<EnemyAI>().enabled = false;
        GetComponent<NavMeshAgent>().enabled = false;
        StartCoroutine(playDyingAnimation());

        scoreUi = (RectTransform) GameObject.FindGameObjectWithTag("ScoreUI").transform;
        scoreUi.GetComponent<ScoreUI>().IncreaseScore(1);

    }


    public IEnumerator playDyingAnimation()
    {
        deathSound.enabled = true;

        yield return new WaitForSeconds(dieAnimationDelay);
        if (healthbar != null)
        {
            healthbar.gameObject.SetActive(false);
        }
        boneShattersound.enabled = true;
        yield return new WaitForSeconds(disappearDelay);
        GameObject.Destroy(gameObject);


    }

}
