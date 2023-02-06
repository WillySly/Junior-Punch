using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ScoreUI : MonoBehaviour
{
    int score = 0;
    TMP_Text scoreText;

    void Start()
    {
        EnemyHealth.enemyDeathEvent += IncreaseScore;
        scoreText = GetComponent<TMP_Text>();
        scoreText.text = "Slayed: " + score.ToString();
    }

    public void IncreaseScore()
    {
        score++;
        scoreText.text = "Slayed: " + score.ToString();

    }
}
