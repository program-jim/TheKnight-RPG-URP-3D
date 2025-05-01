using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionPoint : MonoBehaviour
{
    [Header("Transition Info")]
    public string sceneName;
    public TransitionType transitionType;

    public DestinationType destinationType;
    private bool canTrans;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && canTrans)
        {
            Debug.Log("Enter");

            //TODO: SceneController transition
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canTrans = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canTrans = false;
        }
    }
}

public enum TransitionType
{
    SameScene, DifferentScene
}