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
    [SerializeField] GameObject gameOverCanvas;

    public event Action walk;
    public event Action stopMoving;
    public event Action run;
    public event Action stopRunning;

    string playerName = "Duffy the Skeleton Slaya";

    float cameraMouseDistanceFactor = 3f; // safeguard for mouse/camera overlap
    bool rotate = true; // mouse over player hover flag

    private void OnEnable()
    {
        GetComponent<Health>().deathEvent += Die;
    }
    void Start()
    {
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
            walk?.Invoke();
            transform.position += new Vector3(moveHorizontally, 0, moveVertically);
        }
        else
        {
            stopMoving?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            walkingSpeed *= runningFactor;
            run?.Invoke();

        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            walkingSpeed /= runningFactor;
            stopRunning?.Invoke();

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


    protected override void Die()
    {
        base.Die();
        PlayerController pc = GetComponent<PlayerController>();
        pc.enabled = false;

        PlayerCombat pcombat = GetComponent<PlayerCombat>();
        pcombat.enabled = false;

        GetComponent<CapsuleCollider>().enabled = false;

    }

    private void DyingAnimationFinished()
    {
        gameOverCanvas.SetActive(true);
    }

    private void OnDisable()
    {
        GetComponent<Health>().deathEvent -= Die;
    }
}



