using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
[CreateAssetMenu(fileName = "New Character Data", menuName = "Character States/Character Data")]
public class CharacterData_SO : ScriptableObject
{
    [Header("States Info")]
    public int maxHealth;
    public int currentHealth;
    public int baseDefence;
    public int currentDefence;
}
