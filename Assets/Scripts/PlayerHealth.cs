using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : Health
{
    [SerializeField] float dieAnimationDelay = 5f;
    [SerializeField] RectTransform healthUi;
    [SerializeField] GameObject gameOverCanvas;
    [SerializeField] AudioSource deathSound;

    TMP_Text healthText;

    protected override void Start()
    {
        base.Start();
        healthText = healthUi.GetComponent<TMP_Text>(); 
        healthText.text = "HP: " + health.ToString();
    }

    public override void gotHit(int points)
    {
        base.gotHit(points);
        StartCoroutine(ScoreAnimationDelay());
    }

    IEnumerator ScoreAnimationDelay()
    {
        yield return new WaitForSeconds(hitAnimationDelay + healthbarAnimationDelay);
        healthText.text = healthText.text = "HP: " + health.ToString();
    }


    protected override void Die()
    {
        PlayerController pc = GetComponent<PlayerController>();
        pc.enabled = false;
        PlayerCombat pcombat = GetComponent<PlayerCombat>();
        pcombat.enabled = false;
        base.Die();
        deathSound.enabled = true;
        StartCoroutine(PlayDyingAnimation());
    }

    public IEnumerator PlayDyingAnimation()
    {
        yield return new WaitForSeconds(dieAnimationDelay);
        gameOverCanvas.SetActive(true);

    }

 

}
