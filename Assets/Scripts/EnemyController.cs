using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
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
    [SerializeField] float strikeDistance = 2f;

    [SerializeField] public float viewRadius;
    [Range(0, 360)]
    [SerializeField] public float viewAngle;

    enum state {walk, idle, chase,}

    NavMeshAgent navMeshAgent;
    Animator animator;

    int destinationWaypointIndex;          
    float destinationErrorMargin = 0.1f;
    state currentState;
    bool reachedPlayer;


    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        destinationWaypointIndex = 0;
        navMeshAgent.SetDestination(waypoints[destinationWaypointIndex].position);
        reachedPlayer = false;

        SetState(state.idle);
    }

    void Update()
    {
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
                animator.SetBool("isRunning", true);
                break;
            default:
                animator.SetBool("isWalking", false);
                animator.SetBool("isIdle", true);
                break;
        }   
    }

    state GetState()
    {
        return currentState;
    }


    void Patroling()
    {
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance + destinationErrorMargin)
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
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speed;
    }


    void Stop()
    {
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
                    SetState(state.chase);
                }
            }
        }
    }

    void Chase()
    {
        if (!reachedPlayer)
        {
            Move(runSpeed);
            navMeshAgent.SetDestination(target.position);
        }

        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        if (distanceToTarget >= stopChasingDistance)
        {
            SetState(state.idle);
            Stop();
            ReturnToPost();
            return;
        }

        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance + strikeDistance)
        {
            reachedPlayer = true;
            EngageInCombat();
        }
    }


    void ReturnToPost()
    {
        SetState(state.walk);
        Move(speed);
        navMeshAgent.SetDestination(waypoints[destinationWaypointIndex].position);
    }


    void EngageInCombat()
    {

    }

}
