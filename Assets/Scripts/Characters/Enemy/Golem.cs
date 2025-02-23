using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : EnemyController
{
    [Header("Skill")]
    public float kickForce = 25f;

    public void KickOff()
    {
        if (attackTarget != null && transform.IsFacingTarget(attackTarget.transform))
        {
            var targetStates = attackTarget.GetComponent<CharacterStates>();

            Vector3 direction = attackTarget.transform.position - transform.position;
            Vector3 

            targetStates.TakeDamage(characterStates, targetStates);
        }
    }
}
