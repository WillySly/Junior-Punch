using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] float speed = 10;
    [SerializeField] int accelerationFactor = 3;
    [SerializeField] float health = 100;
    [SerializeField] float punchRange = 0.1f;

    // Lock/Unclock Camera


    //Punch an enemy


    

    void Start()
    {
        
    }

    void Update()
    {
        handleInput();
    }


    void handleInput()
    {
        float speedMultiplier = speed * Time.deltaTime;
        float moveHorizontally = Input.GetAxis("Horizontal")*speedMultiplier;
        float moveVertically = Input.GetAxis("Vertical")* speedMultiplier;

        transform.Translate(moveHorizontally, 0, moveVertically);

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            speed *= accelerationFactor;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed /= accelerationFactor;
        }

        if (Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Space))
        {
            Debug.Log("PUNCH!");
            punch();
        }
    }
    void punch()
    {
        // if looking at enemy and enemy is < punchdistance, hit

  
        
    }
}



