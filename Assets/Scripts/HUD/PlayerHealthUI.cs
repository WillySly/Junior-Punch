using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] Transform player;
    int health = 200;
    TMP_Text healthText;

    void Start()
    {

        health = player.GetComponent<PlayerHealth>().GetHealth();
        healthText = GetComponent<TMP_Text>();
        healthText.text = "HP: " + health.ToString();
        PlayerHealth.playerDamageEvent += DecreaseHealth;

    }

    public void DecreaseHealth(int delta, float delay)
    {
        health -= delta;
        if (health < 0) health = 0;
        StartCoroutine(DisplayHealth(delay));
    }

    IEnumerator DisplayHealth(float delay)
    {
        yield return new WaitForSeconds(delay);
        healthText.text = "HP: " + health.ToString();
    }

    public void OnDestroy()
    {
        PlayerHealth.playerDamageEvent -= DecreaseHealth;
    }
}
