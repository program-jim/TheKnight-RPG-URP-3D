using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(CharacterStates))]
public class EnemyController : MonoBehaviour
{
    public EnemyStates EnemyStates
    {
        get
        {
            return enemyStates;
        }
    }

    private NavMeshHit hit;
    private EnemyStates enemyStates;
    private NavMeshAgent agent;
    private GameObject attackTarget;
    private Animator anim;
    private Vector3 wayPoint;
    private Vector3 guardPos;
    private float speed;
    private float remainLookAtTime;

    public GizmosToDraw gizmos;

    [Header("Basic Settings")]
    public float sightRadius;
    public float lookAtTime = 3f;
    public bool isGuard;
    public bool hasFoundPlayer
    {
        get
        {
            return FoundPlayer();
        }
    }
    // Bool with Animator
    public bool isWalk;
    public bool isChase;
    public bool isFollow;

    [Header("Patrol State")]
    public float patrolRange;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        guardPos = transform.position;
        speed = agent.speed;
        remainLookAtTime = lookAtTime;
    }

    void Start()
    {
        if (isGuard)
        {
            enemyStates = EnemyStates.GUARD;
        }
        else
        {
            GetNewWayPoint();
            enemyStates = EnemyStates.PATROL;
        }
    }

    void Update()
    {
        SwitchStates();
        SwitchAnimation();
    }

    void OnDrawGizmosSelected()
    {
        switch (gizmos)
        {
            case GizmosToDraw.SIGHT_RADIUS:
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(transform.position, sightRadius);
                break;

            case GizmosToDraw.PATROL_RANGE:
                Gizmos.color = Color.magenta;
                Gizmos.DrawWireSphere(transform.position, patrolRange);
                break;
        }
    }

    private void SwitchStates()
    {
        // Switch to CHASE after finding Player.
        if (hasFoundPlayer)
        {
            enemyStates = EnemyStates.CHASE;
            //Debug.Log("Find Player !!!");
        }
        
        switch (enemyStates)
        {
            case EnemyStates.GUARD:
                break;

            case EnemyStates.PATROL:
                Patrol();
                break;

            case EnemyStates.CHASE:
                ChaseTarget();
                break;

            case EnemyStates.DEAD:
                break;
        }
    }

    private void SwitchAnimation()
    {
        anim.SetBool("Walk", isWalk);
        anim.SetBool("Chase", isChase);
        anim.SetBool("Follow", isFollow);
    }

    private bool FoundPlayer()
    {
        var colliders = Physics.OverlapSphere(transform.position, sightRadius);

        foreach (var target in colliders)
        {
            if (target.CompareTag("Player"))
            {
                attackTarget = target.gameObject;
                return true;
            }
        }

        attackTarget = null;
        return false;
    }

    private void ChaseTarget()
    {
        //TODO:Attack target in sight radius
        //TODO:Do with animation
        
        isWalk = false;
        isChase = true;
        agent.speed = speed;
        
        if (!hasFoundPlayer)
        {
            isFollow = false;
            
            if (remainLookAtTime > 0f)
            {
                agent.destination = transform.position;
                remainLookAtTime -= Time.deltaTime;
            }
            else if (isGuard)
            {
                enemyStates = EnemyStates.GUARD;
            }
            else
            {
                enemyStates = EnemyStates.PATROL;
            }
        }
        else
        {
            isFollow = true;
            agent.destination = attackTarget.transform.position;
        }
    }

    private void Patrol()
    {
        isChase = false;
        agent.speed = speed * 0.5f;

        // If arriving at patrol point
        if (Vector3.Distance(wayPoint, transform.position) <= agent.stoppingDistance)
        {
            isWalk = false;
            if (remainLookAtTime > 0)
            {
                remainLookAtTime -= Time.deltaTime;
            }
            else
            {
                GetNewWayPoint();
            }
        }
        else
        {
            isWalk = true;
            agent.destination = wayPoint;
        }
    }

    private void GetNewWayPoint()
    {
        remainLookAtTime = lookAtTime;
        
        float randomX = Random.Range(-patrolRange, patrolRange);
        float randomZ = Random.Range(-patrolRange, patrolRange);
        Vector3 randomPoint = new Vector3(guardPos.x + randomX, transform.position.y, guardPos.z + randomZ);

        // May cause some problems
        wayPoint = NavMesh.SamplePosition(randomPoint, out hit, patrolRange, 1) ? hit.position : transform.position;
    }
}

public enum EnemyStates
{
    GUARD,
    PATROL,
    CHASE,
    DEAD
}

public enum GizmosToDraw
{
    SIGHT_RADIUS,
    PATROL_RANGE
}
