using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] float walkingSpeed = 10;
    [SerializeField] int runningFactor = 3;
    [SerializeField] float health = 100;
    [SerializeField] float punchRange = 0.1f;
    [SerializeField] float dist = 10f;
    Animator animator;

    float cameraMouseDistanceFactor = 3f; // safeguard for mouse/camera overlap
    bool rotate = true; // mouse over player hover flag


    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        HandleInput();
    }


    void HandleInput()
    {
        RotateTowardsMouse();

        float speedMultiplier = walkingSpeed * Time.deltaTime;

        float moveHorizontally = Input.GetAxis("Horizontal") * speedMultiplier;
        float moveVertically = Input.GetAxis("Vertical") * speedMultiplier;

        if (moveHorizontally != 0 || moveVertically != 0)
        {
            animator.SetBool("isMoving", true);
            transform.position += new Vector3(moveHorizontally, 0, moveVertically);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            walkingSpeed *= runningFactor;
            animator.SetBool("isRunning", true);
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            walkingSpeed /= runningFactor;
            animator.SetBool("isRunning", false);
        }

    }

    // Character spins around uncontrollably when mouse hovers over it,
    // So we disable rotation. 

    private void OnMouseEnter()
    {
        rotate = false;
    }

    private void OnMouseExit()
    {
        rotate = true;
    }


    // 
    private void RotateTowardsMouse()
    {
        // check for mouse hover
        if (!rotate) return;

        Vector3 mousePosition = Vector3.zero;
        Vector3 playerPosition = transform.position;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit))
            mousePosition = raycastHit.point + Vector3.up * cameraMouseDistanceFactor;

        Vector3 direction = new Vector3(mousePosition.x - playerPosition.x, 0, mousePosition.z - playerPosition.z);
        transform.forward = direction;

    }

}



