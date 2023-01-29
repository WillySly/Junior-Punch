using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Transform[] waypoints;
    [SerializeField] LayerMask playerMask;
    [SerializeField] LayerMask obstacleMask;

    [SerializeField] float speed = 5;
    [SerializeField] float runSpeed = 5;
    [SerializeField] float idleCounter = 1.2f;
    [SerializeField] float waitTime = 1.2f;
    [SerializeField] float stopChasingDistance = 25f;
    [SerializeField] float destinationErrorMargin = 3f;

    [SerializeField] public float viewRadius;
    [Range(0, 360)]
    [SerializeField] public float viewAngle;

    enum state { walk, idle, chase, engagedInCombat }

    NavMeshAgent navMeshAgent;
    Animator animator;
    EnemyCombat enemyCombat;


    int destinationWaypointIndex;
    
    float rotationSpeed = 10f;

    bool reachedPlayer;
    bool canMove = true;
    Vector3 targetLastPosition;
    state currentState;



    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        enemyCombat = GetComponent<EnemyCombat>();

        destinationWaypointIndex = 0;
        navMeshAgent.SetDestination(waypoints[destinationWaypointIndex].position);
        navMeshAgent.autoBraking = true;
        navMeshAgent.stoppingDistance = destinationErrorMargin;
        reachedPlayer = false;
        SetState(state.idle);
    }

    void Update()
    {
        //Debug.Log(Time.time + " EnemyAI: currentState " + currentState);

        CheckEnvironment();

        if (currentState == state.chase)
        {
            Chase();
        }
        else
        {
            Patroling();
        }
    }

    public Vector3 DirFromAngle(float angle, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angle += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));

    }

    void SetState(state state)
    {
        currentState = state;
        switch (state)
        {
            case state.walk:
                animator.SetBool("isWalking", true);
                animator.SetBool("isIdle", false);
                animator.SetBool("isRunning", false);

                break;
            case state.idle:
                animator.SetBool("isWalking", false);
                animator.SetBool("isIdle", true);
                animator.SetBool("isRunning", false);

                break;
            case state.chase:
                animator.SetBool("isWalking", false);
                animator.SetBool("isIdle", false);
                if (!reachedPlayer)
                {
                    //Debug.Log(Time.time + "running animation starting");
                    animator.SetBool("isRunning", true);
                }
                break;
            //case state.engagedInCombat:
            //    animator.SetBool("isRunning", false);
            //    break;
            default:
                animator.SetBool("isWalking", false);
                animator.SetBool("isIdle", true);
                break;
        }
    }

    state GetCurrentState()
    {
        return currentState;
    }



    void Patroling()
    {
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            if (idleCounter <= 0)
            {
                SetNextWaypoint();
                Move(speed);
                SetState(state.walk);
                idleCounter = waitTime;
            }
            else
            {
                Stop();
                idleCounter -= Time.deltaTime;
                SetState(state.idle);

            }
        }
    }

    void Move(float speed)
    {
        //Debug.Log(Time.time + "in Move: " + speed);
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speed;

        //Debug.Log(Time.time + " in Move: isStopped is " + navMeshAgent.isStopped + " speed is " + speed );

    }


    void Stop()
    {
        //Debug.Log(Time.time + "STOPPED!");

        navMeshAgent.isStopped = true;
        navMeshAgent.speed = 0;
    }

    void SetNextWaypoint()
    {
        destinationWaypointIndex++;
        destinationWaypointIndex %= waypoints.Length;
        navMeshAgent.SetDestination(waypoints[destinationWaypointIndex].position);
    }


    void CheckEnvironment()
    {
        Collider[] targetsInView = Physics.OverlapSphere(transform.position, viewRadius, playerMask);

        for (int i = 0; i < targetsInView.Length; i++)
        {
            Transform target = targetsInView[i].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < viewAngle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstacleMask))
                {
                    //Debug.Log(Time.time + " PLAYER IS HERE");

                    SetState(state.chase);
                }

            }
        }
    }

    // FIX: 1.  After hitting the player, if player moves enemy turns and slides for some time before
    //          running animation starts. Animation is triggered correctly, all the flags are set.
    //
    //      2.  sometimes it gets stuck in running in one place when the player is near it and doesn't move.
    //          Moving triggers repositioning of destination and it starts moving towards the player again.
    //      

    public void Chase()
    {

        if (IsInMeleeRangeOf(target))
        {
            RotateTowards(target);
        }

        // if the player moved during attack
        if (target.position != targetLastPosition)
        {
            reachedPlayer = false;
        }

        if (!reachedPlayer)
        {
            //Debug.Log(Time.time + " Player not reached, can move is " + canMove);
            // if in attack animation, wait for animation to stop and chase again
            // if attack didn't happen yet, canMove is set to true by default
            if (canMove)
            {
                SetState(state.chase);
                navMeshAgent.SetDestination(target.position);
                targetLastPosition = target.position;
                //Debug.Log(Time.time + " Starting movement ");
                Move(runSpeed);

            }
        }

        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        // if player is to far away, stop chasing and go back to patroling
        if (distanceToTarget >= stopChasingDistance)
        {
            SetState(state.idle);
            Stop();
            ReturnToPost();
            return;
        }

        // if player within attack range, attack
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance + enemyCombat.GetAttackRange())
        {
            reachedPlayer = true;
            Stop();
            enemyCombat.EngageInCombat();
            //Debug.Log(Time.time + "Engaging");
        }

    }

    void ReturnToPost()
    {
        SetState(state.walk);
        Move(speed);
        navMeshAgent.SetDestination(waypoints[destinationWaypointIndex].position);
    }


    private bool IsInMeleeRangeOf(Transform target)
    {
        float distance = Vector3.Distance(transform.position, target.position);
        return distance < enemyCombat.GetAttackRange();
    }

    private void RotateTowards(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }


    // catches attack animation ending event. Enemy must not move during attack
    public void AttackEnded()
    {
        canMove = true;
        //Debug.Log(Time.time + " Attack ended, canMove is true");
    }

    public void AttackStarted()
    {
        canMove = false;
        //Debug.Log(Time.time + " Attack started, canMove is false");

    }


}
