using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Controller of the Player.
/// </summary>
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(CharacterStates))]
public class PlayerController : MonoBehaviour
{
    [HideInInspector] public float distance = 1f;
    
    private NavMeshAgent agent;
    private Animator anim;
    private GameObject attackTarget;
    private CharacterStates characterStates;
    private float lastAttackTime;
    private bool isDead;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        characterStates = GetComponent<CharacterStates>();
    }

    private void Start()
    {
        MouseManager.Instance.OnMouseClicked += MoveToTarget;
        MouseManager.Instance.OnEnemyClicked += EventAttack;

        GameManager.Instance.RegisterPlayer(characterStates);
    }

    private void Update()
    {
        CheckDeath();
        SwitchAnimation();
        lastAttackTime -= Time.deltaTime;
    }

    public void MoveToTarget(Vector3 target)
    {
        StopAllCoroutines();
        if (isDead) return;

        agent.isStopped = false;
        agent.destination = target;
    }
    
    private void EventAttack(GameObject target)
    {
        if (isDead) return;
        if (target == null)
        {
            return;
        }

        attackTarget = target;
        characterStates.isCritical = UnityEngine.Random.value < characterStates.attackData.criticalChance;
        StartCoroutine(MoveToAttackTarget());
    }

    private void SwitchAnimation()
    {
        anim.SetFloat("Speed", agent.velocity.sqrMagnitude);
        anim.SetBool("Death", isDead);
    }

    private void CheckDeath()
    {
        isDead = characterStates.CurrentHealth == 0;

        if (isDead)
        {
            GameManager.Instance.NotifyObservers();
        }
    }

    IEnumerator MoveToAttackTarget()
    {
        agent.isStopped = false;
        transform.LookAt(attackTarget.transform);
        // Change radius of Attack.
        distance = Vector3.Distance(transform.position, attackTarget.transform.position);

        while (distance > characterStates.attackData.attackRange)
        {
            agent.destination = attackTarget.transform.position;
            yield return null;
        }

        agent.isStopped = true;
        // Attack

        if (lastAttackTime < 0f)
        {
            anim.SetBool("Critical", characterStates.isCritical);
            anim.SetTrigger("Attack");

            //TODO: Reset CD Time
            lastAttackTime = characterStates.attackData.coolDown;
        }
    }

    /// <summary>
    /// Hit others (Animation Event Function).
    /// </summary>
    public void Hit()
    {
        var targetStates = attackTarget.GetComponent<CharacterStates>();
        targetStates.TakeDamage(characterStates, targetStates);
    }
}
