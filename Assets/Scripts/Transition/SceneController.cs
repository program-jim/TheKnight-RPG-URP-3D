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
                
                break;

            case TransitionType.DifferentScene:
                break;
        }
    }

    IEnumerator Transition(string sceneName, DestinationType destinationType)
    {
        player = GameManager.Instance.playerStates.gameObject;
        player.transform.SetPositionAndRotation();

    }

    private TransitionDestination GetTransitionDestination(DestinationType destinationType)
    {
        var entrances = FindObjectsOfType<TransitionDestination>();



        return null;
    }
}
