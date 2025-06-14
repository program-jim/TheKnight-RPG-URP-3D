using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Grunt : EnemyController
{
    [Header("Skill")]
    public float kickForce = 10f;

    public void KickOff()
    {
        if (attackTarget == null)
        {
            return;
        }
        
        transform.LookAt(attackTarget.transform);
        
        var direction = attackTarget.transform.position - transform.position;
        direction.Normalize();

        attackTarget.GetComponent<NavMeshAgent>().isStopped = true;
        attackTarget.GetComponent<NavMeshAgent>().velocity = direction * kickForce;
        attackTarget.GetComponent<Animator>().SetTrigger("Dizzy");
    }
}
