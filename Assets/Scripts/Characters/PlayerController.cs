using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
public class PlayerController : MonoBehaviour
{
    public float attackDistance = 1f;
    [HideInInspector] public float distance = 1f;
    
    private NavMeshAgent agent;
    private Animator anim;
    private GameObject attackTarget;
    private float lastAttackTime;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        MouseManager.Instance.OnMouseClicked += MoveToTarget;
        MouseManager.Instance.OnEnemyClicked += EventAttack;
    }

    private void Update()
    {
        SwitchAnimation();
        lastAttackTime -= Time.deltaTime;
    }

    public void MoveToTarget(Vector3 target)
    {
        StopAllCoroutines();
        agent.isStopped = false;
        agent.destination = target;
    }
    
    private void EventAttack(GameObject target)
    {
        if (target == null)
        {
            return;
        }

        attackTarget = target;
        StartCoroutine(MoveToAttackTarget());
    }

    public void SwitchAnimation()
    {
        anim.SetFloat("Speed", agent.velocity.sqrMagnitude);
    }

    IEnumerator MoveToAttackTarget()
    {
        agent.isStopped = false;
        transform.LookAt(attackTarget.transform);
        // Change radius of Attack.
        distance = Vector3.Distance(transform.position, attackTarget.transform.position);

        while (distance > attackDistance)
        {
            agent.destination = attackTarget.transform.position;
            yield return null;
        }

        agent.isStopped = true;
        // Attack

        if (lastAttackTime < 0f)
        {
            anim.SetTrigger("Attack");

            //TODO: Reset CD Time
            lastAttackTime = 0.5f;
        }
    }
}
