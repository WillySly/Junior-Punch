using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class GameOver : MonoBehaviour
{
    string finalText = "Overconfidence is a slow and insidious killer...\n\nESC - to quit     SPACE - to continue";

    // Start is called before the first frame update
    void Start()
    {
        TMP_Text gameOverText = GetComponent<TMP_Text>();
        gameOverText.text = finalText;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            Debug.Log("escape");
            Application.Quit();
        }

    }

}

// Update is called once per frame
