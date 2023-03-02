using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarUI : MonoBehaviour
{
    [SerializeField] Image healthbarForegroundImage;
    [SerializeField] float healthbarUpdateSpeedSeconds = 0.5f;
    [SerializeField] protected Transform character;


    int initHealth;

    protected virtual void OnEnable()
    {
        character.GetComponent<Health>().updateHealthEvent += UpdateHealth;
    }

    void Start()
    {
        initHealth = character.GetComponent<Health>().health;
    }

    void  UpdateHealth(int health)
    {
        StartCoroutine(UpdateHealthbar(health));
    }

    IEnumerator UpdateHealthbar(int health)
    {
            float pct = (float)health / (float)initHealth;
            float currHealth = healthbarForegroundImage.fillAmount;
            float elapsed = 0f;

            while (elapsed < healthbarUpdateSpeedSeconds)
            {
                elapsed += Time.deltaTime;
                healthbarForegroundImage.fillAmount = Mathf.Lerp(currHealth, pct, elapsed / healthbarUpdateSpeedSeconds);
                yield return null;
            }

            healthbarForegroundImage.fillAmount = pct;      
    }

    void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
    }

     protected virtual void OnDisable()
    {
        character.GetComponent<Health>().updateHealthEvent -= UpdateHealth;  
            
    }


}
