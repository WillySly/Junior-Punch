using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class EnemyAI : Character
{
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
    [SerializeField] float sneakingDistance = 5f;

    List<Transform> waypoints;
    Transform player;
    NavMeshAgent navMeshAgent;
    EnemyCombat enemyCombat;

    public event Action walk;
    public event Action idle;
    public event Action<bool> chase;
    public event Action engageInCombat;

    enum state { walk, idle, chase }
    state currentState;


    int destinationWaypointIndex;
    float rotationSpeed = 10f;

    bool reachedPlayer;
    bool animationPlaying;

    Vector3 targetLastPosition; // to check whether player moved

    private void OnEnable()
    {
        GetComponent<EnemyFXController>().isBusy += AnimationPlaying;
        GetComponent<Health>().deathEvent += Die;

    }

    void Start()
    {
        SetState(state.idle);
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyCombat = GetComponent<EnemyCombat>();

        destinationWaypointIndex = 0;
        navMeshAgent.autoBraking = true;
        navMeshAgent.stoppingDistance = destinationErrorMargin;
        reachedPlayer = false;
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

    void CheckEnvironment()
    {
        Collider[] targetsInView = Physics.OverlapSphere(transform.position, viewRadius, playerMask);

        for (int i = 0; i < targetsInView.Length; i++)
        {
            Transform target = targetsInView[i].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            float distanceToTarget = Vector3.Distance(transform.position, target.position);

            if (Vector3.Angle(transform.forward, directionToTarget) < viewAngle / 2 || distanceToTarget < sneakingDistance)
            {
                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstacleMask))
                {
                    player = target;
                    SetState(state.chase);
                }

            }
        }
    }


    // Handles SFX and AFX 
    void SetState(state state)
    {
        currentState = state;
        switch (state)
        {
            case state.walk:
                walk?.Invoke();
                break;
            case state.idle:
                idle?.Invoke();
                break;
            case state.chase:
                chase?.Invoke(reachedPlayer);
                break;
            default:
                idle?.Invoke();
                break;
        }
    }


    public void Chase()
    {
        if (player != null)
        {
            if (IsInMeleeRangeOf(player))
            {
                engageInCombat?.Invoke();
                RotateTowards(player);
            }

            // if the player moved during attack
            if (player.position != targetLastPosition)
            {
                reachedPlayer = false;
            }

            if (!reachedPlayer)
            {
                // if in attack or damage animation, wait for animation to stop and chase again

                if (!animationPlaying)
                {
                    SetState(state.chase);
                    navMeshAgent.SetDestination(player.position);
                    targetLastPosition = player.position;
                    Move(runSpeed);
                }
            }

            float distanceToTarget = Vector3.Distance(transform.position, player.position);

            // if player is to far away, stop chasing and go back to patroling
            if (distanceToTarget >= stopChasingDistance)
            {
                SetState(state.idle);
                Stop();
                ReturnToPost();
                return;
            }

            // if player within attack range, attack
            if (IsInMeleeRangeOf(player))
            {
                if (!animationPlaying)
                {
                    Stop();
                    reachedPlayer = true;
                    enemyCombat.EngageInCombat();
                }
            }
        }

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

    void ReturnToPost()
    {
        SetState(state.walk);
        Move(speed);
        navMeshAgent.SetDestination(waypoints[destinationWaypointIndex].position);
    }

    bool IsInMeleeRangeOf(Transform target)
    {
        float distance = Vector3.Distance(transform.position, target.position);
        return distance < enemyCombat.GetAttackRange();
    }

    void RotateTowards(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    protected override void Die()
    {

            
            base.Die();
            GetComponent<EnemyAI>().enabled = false;
            GetComponent<NavMeshAgent>().enabled = false;
            GetComponent<CapsuleCollider>().enabled = false;

        Debug.Log("died");
        
        StartCoroutine(Disappear());
        
    }

    public void SetWaypoints(List<Transform> wp)
    {
        waypoints = wp;
    }

    private void AnimationPlaying(bool playing)
    {
        animationPlaying = playing;
    }

    public Vector3 DirFromAngle(float angle, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angle += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
    }


    private void OnDisable()
    {
        GetComponent<EnemyFXController>().isBusy -= AnimationPlaying;
        GetComponent<Health>().deathEvent -= Die;


    }
}
