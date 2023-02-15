using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class PlayerController : Character
{

    [SerializeField] float walkingSpeed = 10;
    [SerializeField] int runningFactor = 3;     // running speed multiplier
    [SerializeField] TMP_Text charName;

    PlayerFXController FXController;
     
    string playerName = "Duffy the Skeleton Slaya";
    [SerializeField] GameObject gameOverCanvas;

    float cameraMouseDistanceFactor = 3f; // safeguard for mouse/camera overlap
    bool rotate = true; // mouse over player hover flag

    public static event Action playerDeathEvent;


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
            FXController.Walk();
            transform.position += new Vector3(moveHorizontally, 0, moveVertically);
        }
        else
        {
            FXController.StopMoving();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            walkingSpeed *= runningFactor;
            FXController.Run();

        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            walkingSpeed /= runningFactor;
            FXController.StopRunning();
  
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


    protected override void Die(GameObject gameObject)
    {
        if (gameObject == this.gameObject)
        {
            PlayerController pc = GetComponent<PlayerController>();
            pc.enabled = false;

            PlayerCombat pcombat = GetComponent<PlayerCombat>();
            pcombat.enabled = false;

            GetComponent<CapsuleCollider>().enabled = false;

            base.Die(gameObject);

            if (playerDeathEvent != null)
            {
                playerDeathEvent();
            }
        }
       

    }

        private void DyingAnimationFinished()
    {
        gameOverCanvas.SetActive(true);
    }


}



