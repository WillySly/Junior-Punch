using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class GameOver : MonoBehaviour
{
    string finalText = "Overconfidence is a slow and insidious killer...\n\nESC - to quit     SPACE - to continue";

    void Start()
    {
        TMP_Text gameOverText = GetComponent<TMP_Text>();
        gameOverText.text = finalText;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

}

