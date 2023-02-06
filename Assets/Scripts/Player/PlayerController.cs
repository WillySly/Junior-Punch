using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class PlayerController : MonoBehaviour
{

    [SerializeField] float walkingSpeed = 10;
    [SerializeField] int runningFactor = 3;     // running speed multiplier
    [SerializeField] float punchRange = 0.1f;
    [SerializeField] TMP_Text charName;

    PlayerFXController FXController;
     
    string playerName = "Duffy the Skeleton Slaya";

    float cameraMouseDistanceFactor = 3f; // safeguard for mouse/camera overlap
    bool rotate = true; // mouse over player hover flag

    void Start()
    {
        FXController = GetComponent<PlayerFXController>();
        charName.text = playerName;
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
            FXController.walk();
            transform.position += new Vector3(moveHorizontally, 0, moveVertically);
        }
        else
        {
            FXController.stopMoving();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            walkingSpeed *= runningFactor;
            FXController.run();

        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            walkingSpeed /= runningFactor;
            FXController.stopRunning();
  
        }

    }

    // Character spins around uncontrollably when mouse hovers over it,
    // So we disable rotation. 

    void OnMouseEnter()
    {
        rotate = false;
    }

    void OnMouseExit()
    {
        rotate = true;
    }

    void RotateTowardsMouse()
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



