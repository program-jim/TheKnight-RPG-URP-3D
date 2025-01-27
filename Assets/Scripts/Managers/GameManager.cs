using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Manager of the game.
/// </summary>
public class GameManager : SingletonMono<GameManager>
{
    public CharacterStates playerStates;

    public void RegisterPlayer(CharacterStates player)
    {
        playerStates = player;
    }
}
