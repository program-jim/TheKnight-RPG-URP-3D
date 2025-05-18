using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/// <summary>
/// The Manager of the game.
/// </summary>
public class GameManager : SingletonMono<GameManager>
{
    [HideInInspector] public CharacterStates playerStates;
    public List<IEndGameObserver> endGameObservers = new List<IEndGameObserver>();
    private CinemachineFreeLook followCamera;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    public void RegisterPlayer(CharacterStates player)
    {
        playerStates = player;
        followCamera = FindObjectOfType<CinemachineFreeLook>();

        if (followCamera != null)
        {
            followCamera.Follow = playerStates.transform.GetChild(2);
            followCamera.LookAt = playerStates.transform.GetChild(2);
        }
    }

    public void AddObserver(IEndGameObserver observer)
    {
        endGameObservers.Add(observer);
    }

    public void RemoveObserver(IEndGameObserver observer)
    {
        endGameObservers.Remove(observer);
    }

    public void NotifyObservers()
    {
        foreach (var observer in endGameObservers)
        {
            observer.EndNotify();
        }
    }

    public Transform GetEntrance()
    {
        foreach (var item in FindObjectsOfType<TransitionDestination>())
        {
            if (item.destinationType == DestinationType.ENTER)
            {
                return item.transform;
            }
        }

        return null;
    }
}
