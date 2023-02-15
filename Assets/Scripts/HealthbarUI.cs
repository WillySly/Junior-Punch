using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarUI : MonoBehaviour
{
    [SerializeField] Image healthbarForegroundImage;
    [SerializeField] float healthbarUpdateSpeedSeconds = 0.5f;
    [SerializeField] Transform character;


    Animator animator;
    int initHealth;

    private void OnEnable()
    {
        Health.updateHealthEvent += UpdateHealth;
        EnemyFXController.enemyFallEvent += EnemyFallEvent;
    }

    void Start()
    {
        animator = character.GetComponent<Animator>();
        initHealth = character.GetComponent<Health>().health;
    }

    void  UpdateHealth(int health, GameObject gameObject)
    {
        StartCoroutine(UpdateHealthbar(health, gameObject));
    }

    IEnumerator UpdateHealthbar(int health, GameObject gameObject)
    {
        if (gameObject == character.gameObject)
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
            animator.ResetTrigger("isHurt");
        }

    }
    void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
    }

    private void OnDisable()
    {
        Health.updateHealthEvent -= UpdateHealth;
        EnemyFXController.enemyFallEvent -= EnemyFallEvent;
    }

    private void EnemyFallEvent()
    {
        if (transform != null)
        {
            transform.gameObject.SetActive(false);
        }
    }
}
