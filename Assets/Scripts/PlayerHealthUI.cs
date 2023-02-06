using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class PlayerHealthUI : MonoBehaviour
{
    int health = 200;
    TMP_Text healthText;

    void Start()
    {
        PlayerHealth.playerHitEvent += DecreaseHealth;
        healthText = GetComponent<TMP_Text>();
        healthText.text = "HP: " + health.ToString();
    }

    public void DecreaseHealth(int delta, float delay)
    {
        health -= delta;
        StartCoroutine(DisplayHealth(delay));
    }

    IEnumerator DisplayHealth(float delay)
    {
        yield return new WaitForSeconds(delay);
        healthText.text = "HP: " + health.ToString();
    }
}
