using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class SceneController : SingletonMono<SceneController>
{
    private GameObject player;
    private NavMeshAgent playerAgent;


    public void TransitionToDestination(TransitionPoint transitionPoint)
    {
        switch (transitionPoint.transitionType)
        {
            case TransitionType.SameScene:
                StartCoroutine(Transition(SceneManager.GetActiveScene().name, transitionPoint.destinationType));
                break;

            case TransitionType.DifferentScene:
                break;
        }
    }

    IEnumerator Transition(string sceneName, DestinationType destinationType)
    {
        player = GameManager.Instance.playerStates.gameObject;
        playerAgent = player.GetComponent<NavMeshAgent>();

        // Turn off player's agent.
        playerAgent.enabled = false;
        player.transform.SetPositionAndRotation(GetTransitionDestination(destinationType).transform.position, GetTransitionDestination(destinationType).transform.rotation);

        // Turn on player's agent back.
        playerAgent.enabled = true;
        yield return null;
    }

    private TransitionDestination GetTransitionDestination(DestinationType destinationType)
    {
        var entrances = FindObjectsOfType<TransitionDestination>();

        foreach (var item in entrances)
        {
            if (item.destinationType == destinationType)
            {
                return item;
            }
        }

        return null;
    }
}
