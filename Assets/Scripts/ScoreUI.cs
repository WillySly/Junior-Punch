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
        scoreText = GetComponent<TMP_Text>();
        scoreText.text = "Slayed: " + score.ToString();
    }

    public void IncreaseScore(int delta)
    {
        score += delta;
        scoreText.text = "Slayed: " + score.ToString();

    }
}
