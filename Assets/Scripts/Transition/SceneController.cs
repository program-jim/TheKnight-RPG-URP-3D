using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : SingletonMono<SceneController>
{
    private GameObject player;
    
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
        player.transform.SetPositionAndRotation(GetTransitionDestination(destinationType).transform.position, GetTransitionDestination(destinationType).transform.rotation);
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
