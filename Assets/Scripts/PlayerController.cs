using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] float walkingSpeed = 10;
    [SerializeField] int runningFactor = 3;
    [SerializeField] float health = 100;
    [SerializeField] float punchRange = 0.1f;


    Animator animator;

    bool isMoving = false;
    bool isRunning = false;

    // Lock/Unclock Camera


    //Punch an enemy


    

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        handleInput();
    }


    void handleInput()
    {
        float speedMultiplier = walkingSpeed * Time.deltaTime;


        float moveHorizontally = Input.GetAxis("Horizontal") * speedMultiplier;
        float moveVertically = Input.GetAxis("Vertical") * speedMultiplier;


       

        Debug.Log("moveHorizontally: " + moveHorizontally);
        Debug.Log("moveVertically: " + moveVertically);
        Vector3 mousePosition = Vector3.zero;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            mousePosition = raycastHit.point;
        }

        //Debug.Log("mouse position: " + mousePosition);
        Vector3 playerPosition = transform.position;

        Vector3 direction = new Vector3(mousePosition.x - playerPosition.x, 0, mousePosition.z - playerPosition.z);
        transform.forward = direction;

        float angle = Mathf.Atan2(playerPosition.z - mousePosition.y, playerPosition.x - mousePosition.x) * Mathf.Rad2Deg;


        //transform.Rotate(0, angle, 0, Space.Self );
        Debug.Log("angle: " + angle);


        if (moveHorizontally != 0 || moveVertically != 0)
        {
            animator.SetBool("isMoving", true);
            transform.Translate(moveHorizontally, 0, moveVertically);

        }
        else
        {
            animator.SetBool("isMoving", false);
            Debug.Log("is moving to false");
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Debug.Log("shift down");
            walkingSpeed *= runningFactor;
            animator.SetBool("isRunning", true);
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            Debug.Log("shift up");

            walkingSpeed /= runningFactor;
            animator.SetBool("isRunning", false);

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



