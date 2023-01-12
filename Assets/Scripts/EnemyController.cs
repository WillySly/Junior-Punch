using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    enum Direction { North, East, South, West };
    [SerializeField] float health = 30.0f;
    [SerializeField] int period;
    [SerializeField] Vector3 movementVector;
    [SerializeField] PlayerController player;

    float movementFactor;
    //float turnSpeed = 10.0f;
    float visualFieldRadius = 5.0f;
    Vector3 spawnPosition;
    //Flags
    Vector3 moveDirection;



    //
    void Start()
    {
        spawnPosition = transform.position;

    }

    void Update()
    {
        pace();
        checkAndChase();
    }



    void pace()
    {
        float cycles = Time.time / period;
        const float tau = Mathf.PI * 2;

        float rawSinWave = Mathf.Sin(cycles * tau);

        movementFactor = (rawSinWave + 1f) / 2f;

        Vector3 offset = movementVector * movementFactor;
        transform.position = spawnPosition + offset;
        
    }

    void checkAndChase()
    {

        if (Vector3.Distance(player.transform.position, transform.position) <= visualFieldRadius)
        {
            Debug.Log("CHASE!");
            //turnToPlayer
            //chasePlayer

        }
        // if player in visual area - chase

        // if in punching range - punch

    }
}
