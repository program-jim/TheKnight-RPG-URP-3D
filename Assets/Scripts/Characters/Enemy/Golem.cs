using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Golem : EnemyController
{
    [Header("Skill")]
    public float kickForce = 25f;
    public GameObject rockPrefab;
    public Transform handPos;

    #region Animation Events

    /// <summary>
    /// Animation Event to kick off.
    /// </summary>
    public void KickOff()
    {
        if (attackTarget != null && transform.IsFacingTarget(attackTarget.transform))
        {
            var targetStates = attackTarget.GetComponent<CharacterStates>();

            Vector3 direction = (attackTarget.transform.position - transform.position).normalized;
            targetStates.GetComponent<NavMeshAgent>().isStopped = true;
            targetStates.GetComponent<NavMeshAgent>().velocity = kickForce * direction;
            // Set player to dizzy can be removed
            targetStates.GetComponent<Animator>().SetTrigger("Dizzy");


            targetStates.TakeDamage(characterStates, targetStates);
        }
    }

    /// <summary>
    /// Animation Event to throw rock.
    /// </summary>
    public void ThrowRock()
    {
        if (attackTarget != null)
        {
            var rock = Instantiate(rockPrefab, handPos.position, Quaternion.identity);
            rock.GetComponent<Rock>().target = attackTarget;
        }
    }

    #endregion

}
