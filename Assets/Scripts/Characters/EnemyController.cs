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
    private float speed;

    [Header("Basic Settings")]
    public float sightRadius;
    public bool isGuard;
    public bool hasFoundPlayer
    {
        get
        {
            return FoundPlayer();
        }
    }

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        speed = agent.speed;
    }

    void Update()
    {
        SwitchStates();
    }

    private void SwitchStates()
    {
        // Switch to CHASE after finding Player.
        if (hasFoundPlayer)
        {
            enemyStates = EnemyStates.CHASE;
            Debug.Log("Find Player !!!");
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
        
        agent.speed = speed;
        
        if (!hasFoundPlayer)
        {

        }
        else
        {
            agent.destination = attackTarget.transform.position;

        }
    }
}

public enum EnemyStates
{
    GUARD,
    PATROL,
    CHASE,
    DEAD
}
