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

}

public enum EnemyStates
{
    GUARD,
    PATROL,
    CHASE,
    DEAD
}
