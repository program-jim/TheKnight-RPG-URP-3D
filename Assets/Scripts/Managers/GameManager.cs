using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Manager of the game.
/// </summary>
public class GameManager : SingletonMono<GameManager>
{
    public CharacterStates playerStates;
    public List<IEndGameObserver> endGameObservers = new List<IEndGameObserver>();

    public void RegisterPlayer(CharacterStates player)
    {
        playerStates = player;
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
}
