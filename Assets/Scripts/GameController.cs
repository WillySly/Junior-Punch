using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    [SerializeField] GameObject enemy;
    [SerializeField] int maxEnemies;
    int currentEnemies;

 
 
    void Start()
    {
        
    }

    void Update()
    {
        spawnEnemies();
    }


    void spawnEnemies()
    {
        while (currentEnemies < maxEnemies)
        {
            GameObject newEnemy = GameObject.Instantiate(enemy);
            //newEnemy.transform.position = Random.Range();
        }
    }

}
