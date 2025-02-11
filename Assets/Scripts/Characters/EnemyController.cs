using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Controller of the Enemy.
/// </summary>
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(CharacterStates))]
public class EnemyController : MonoBehaviour, IEndGameObserver
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
    private Collider coll;
    protected GameObject attackTarget;
    private Animator anim;
    private CharacterStates characterStates;
    private Vector3 wayPoint;
    private Vector3 guardPos;
    private Quaternion guardRotation;
    private float speed;
    private float remainLookAtTime;
    private float lastAttackTime;

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
    public bool isWin;
    public bool isDead;
    public bool isPlayerDead;

    [Header("Patrol State")]
    public float patrolRange;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        coll = GetComponent<Collider>();
        anim = GetComponent<Animator>();
        characterStates = GetComponent<CharacterStates>();

        guardPos = transform.position;
        guardRotation = transform.rotation;
        speed = agent.speed;
        remainLookAtTime = lookAtTime;
    }

    //FIXME: CHANGE IN LOADING SCENE
    //private void OnEnable()
    //{
    //    GameManager.Instance.AddObserver(this);
    //}

    private void OnDisable()
    {
        if (!GameManager.isInitialized)
        {
            return;
        }
        
        GameManager.Instance.RemoveObserver(this);
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

        //FIXME: CHANGE IN LOADING SCENE.
        GameManager.Instance.AddObserver(this);
    }

    void Update()
    {
        CheckDeath();

        if (!isPlayerDead)
        {
            SwitchStates();
            SwitchAnimation();

            lastAttackTime -= Time.deltaTime;
        }
        
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
        if (isDead)
        {
            enemyStates = EnemyStates.DEAD;
        }

        // Switch to CHASE after finding Player.
        else if (hasFoundPlayer)
        {
            enemyStates = EnemyStates.CHASE;
            //Debug.Log("Find Player !!!");
        }
        
        switch (enemyStates)
        {
            case EnemyStates.GUARD:
                GuardMode();
                break;

            case EnemyStates.PATROL:
                Patrol();
                break;

            case EnemyStates.CHASE:
                ChaseTarget();
                break;

            case EnemyStates.DEAD:
                GoToDie();
                break;
        }
    }

    private void SwitchAnimation()
    {
        anim.SetBool("Walk", isWalk);
        anim.SetBool("Chase", isChase);
        anim.SetBool("Follow", isFollow);
        anim.SetBool("Critical", characterStates.isCritical);
        anim.SetBool("Win", isWin);
        anim.SetBool("Death", isDead);
    }

    private void CheckDeath()
    {
        if (characterStates.CurrentHealth == 0)
        {
            isDead = true;
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

    private void GuardMode()
    {
        isChase = false;

        if (transform.position != guardPos)
        {
            isWalk = true;
            agent.isStopped = false;
            agent.destination = guardPos;

            if (Vector3.SqrMagnitude(guardPos - transform.position) <= agent.stoppingDistance)
            {
                isWalk = false;
                transform.rotation = Quaternion.Lerp(transform.rotation, guardRotation, 0.01f);
            }
        }
    }

    private void ChaseTarget()
    {      
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
            agent.isStopped = false;
            agent.destination = attackTarget.transform.position;
        }

        if (TargetInAttackRange() || TargetInSkillRange())
        {
            isFollow = false;
            agent.isStopped = true;

            if (lastAttackTime < 0f)
            {
                lastAttackTime = characterStates.attackData.coolDown;

                // Critical damage
                characterStates.isCritical = Random.value < characterStates.attackData.criticalChance;
                // Executing attack
                Attack();
            }
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

        wayPoint = NavMesh.SamplePosition(randomPoint, out hit, patrolRange, 1) ? hit.position : transform.position;
    }

    private void Attack()
    {
        transform.LookAt(attackTarget.transform);

        if (TargetInAttackRange())
        {
            // Do with close attack animation.
            anim.SetTrigger("Attack");
        }
        
        if (TargetInSkillRange())
        {
            // Do with skill attack animation.
            anim.SetTrigger("Skill");
        }
    }

    private void GoToDie()
    {
        coll.enabled = false;
        // agent.enabled = false;
        agent.radius = 0f;
        Destroy(gameObject, 2f);
    }

    private bool TargetInAttackRange()
    {
        if (attackTarget != null)
        {
            return Vector3.Distance(transform.position, attackTarget.transform.position) <= characterStates.attackData.attackRange;
        }
        else
        {
            return false;
        }
    }

    private bool TargetInSkillRange()
    {
        if (attackTarget != null)
        {
            return Vector3.Distance(transform.position, attackTarget.transform.position) <= characterStates.attackData.skillRange;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Hit others (Animation Event Function).
    /// </summary>
    public void Hit()
    {
        if (attackTarget != null)
        {
            var targetStates = attackTarget.GetComponent<CharacterStates>();
            targetStates.TakeDamage(characterStates, targetStates);
        }
    }

    public void EndNotify()
    {
        //Winner Animation
        //Stop all move
        //Stop NavMeshAgent

        isWin = true;
        isPlayerDead = true;
        isChase = false;
        isWalk = false;
        attackTarget = null;

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
