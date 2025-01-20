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

    [Header("Basic Settings")]
    public float sightRadius;
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
                return true;
            }
        }

        return false;
    }
}

public enum EnemyStates
{
    GUARD,
    PATROL,
    CHASE,
    DEAD
}
