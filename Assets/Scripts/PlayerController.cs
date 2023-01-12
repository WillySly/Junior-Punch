using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] float speed;
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
        if (Input.GetKey("w") || Input.GetKey("up"))
            Move(Vector3.forward);
        
        if (Input.GetKey("s") || Input.GetKey("down"))
            Move(Vector3.back);
  
        if (Input.GetKey("a") || Input.GetKey("left"))
            Move(Vector3.left);

        if (Input.GetKey("d") || Input.GetKey("right"))
            Move(Vector3.right);

        if (Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Space))
        {
            Debug.Log("PUNCH!");
            punch();
        }
    }

    void Move(Vector3 direction)
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    void punch()
    {
        // if looking at enemy and enemy is < punchdistance, hit

  
        
    }
}



