using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    public EnemyStates EnemyStates
    {
        get
        {
            return enemyStates;
        }
    }

    private EnemyStates enemyStates;
    private NavMeshAgent agent;
    private GameObject attackTarget;
    private Animator anim;
    private Vector3 wayPoint; 
    private float speed;

    [Header("Basic Settings")]
    public GizmosToDraw gizmos;
    public float sightRadius;
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

        speed = agent.speed;
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
        //TODO:Chase target
        //TODO:Back to previous state
        //TODO:Attack target in sight radius
        //TODO:Do with animation
        
        isWalk = false;
        isChase = true;
        agent.speed = speed;
        
        if (!hasFoundPlayer)
        {
            isFollow = false;
            agent.destination = transform.position;
        }
        else
        {
            isFollow = true;
            agent.destination = attackTarget.transform.position;
        }
    }

    private void GetNewWayPoint()
    {
        float randomX = Random.Range(-patrolRange, patrolRange);
        float randomZ = Random.Range(-patrolRange, patrolRange);

        Vector3 randomPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
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
