using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ScoreUI : MonoBehaviour
{
    int score = 0;
    TMP_Text scoreText;

    private void OnEnable()
    {
        Health.deathStaticEvent += IncreaseScore;
    }
    void Start()
    {
      
        scoreText = GetComponent<TMP_Text>();
        scoreText.text = "Slayed: " + score.ToString();
    }

    public void IncreaseScore()
    {
        score++;
        scoreText.text = "Slayed: " + score.ToString();

    }

    private void OnDisable()
    {
        Health.deathStaticEvent -= IncreaseScore;
    }
}
