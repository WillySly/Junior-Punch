using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] Transform player;
    int health = 200;
    TMP_Text healthText;


    private void OnEnable()
    {
        player.GetComponent<Health>().updateHealthEvent += DecreaseHealth;
    }
    void Start()
    {
        health = player.GetComponent<PlayerHealth>().GetHealth();
        healthText = GetComponent<TMP_Text>();
        healthText.text = "HP: " + health.ToString();

    }

    public void DecreaseHealth(int delta)
    {
        health -= delta;
        if (health < 0) health = 0;
        healthText.text = "HP: " + health.ToString();
    }

 
    public void OnDestroy()
    {
        player.GetComponent<Health>().updateHealthEvent -= DecreaseHealth;
    }
}
