using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] LayerMask playerMask;
    [SerializeField] LayerMask obstacleMask;

    [SerializeField] float speed = 5;
    [SerializeField] float runSpeed = 5;
    [SerializeField] float idleCounter = 1.2f;
    [SerializeField] float waitTime = 1.2f;
    [SerializeField] float stopChasingDistance = 25f;
    [SerializeField] float destinationErrorMargin = 3f;
    [SerializeField] AudioSource walkSound, runSound;

    [SerializeField] public float viewRadius;

    [Range(0, 360)]
    [SerializeField] public float viewAngle;


    List<Transform> waypoints;
    Transform target;
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
        target = GameObject.FindGameObjectWithTag("Player").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        enemyCombat = GetComponent<EnemyCombat>();

        destinationWaypointIndex = 0;
        //navMeshAgent.SetDestination(waypoints[destinationWaypointIndex].position);
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

    public void SetWaypoints(List<Transform> wp)
    {
        waypoints = wp;
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
                Debug.Log("Here");
                walkSound.enabled = true;
                runSound.enabled = false;

                animator.SetBool("isWalking", true);
                animator.SetBool("isIdle", false);
                animator.SetBool("isRunning", false);

                break;
            case state.idle:
                walkSound.enabled = false;
                runSound.enabled = false;
                animator.SetBool("isWalking", false);
                animator.SetBool("isIdle", true);
                animator.SetBool("isRunning", false);

                break;
            case state.chase:
                animator.SetBool("isWalking", false);
                animator.SetBool("isIdle", false);
                if (!reachedPlayer)
                {
                    walkSound.enabled = false;
                    runSound.enabled = true;
                    animator.SetBool("isRunning", true);
                }
                break;
 
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
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speed;

    }


    void Stop()
    {
        navMeshAgent.speed = 0;
        navMeshAgent.isStopped = true;
        navMeshAgent.velocity = Vector3.zero;
   
        
    }

    void SetNextWaypoint()
    {
        destinationWaypointIndex++;
        if (waypoints.Count != 0)
        {
            destinationWaypointIndex %= waypoints.Count;
            navMeshAgent.SetDestination(waypoints[destinationWaypointIndex].position);
        }

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
        if (IsInMeleeRangeOf(target))
            //navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance + enemyCombat.GetAttackRange())
        {
            Stop();
            
            reachedPlayer = true;
            
            enemyCombat.EngageInCombat();
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
    }

    public void AttackStarted()
    {
        canMove = false;

    }

    public void DamageEnded()
    {
        canMove = true;
    }

    public void DamageStarted()
    {
        canMove = false;

    }


}
