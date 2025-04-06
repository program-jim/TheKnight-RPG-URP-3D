using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ScriptableObject which stores data of a character.
/// </summary>
[CreateAssetMenu(fileName = "New Character Data", menuName = "Character States/Character Data")]
public class CharacterData_SO : ScriptableObject
{
    [Header("States Info")]
    public int maxHealth;
    public int currentHealth;
    public int baseDefence;
    public int currentDefence;

    [Header("Kill")]
    public int killPoint;

    [Header("Level")]
    public int currentLevel;
    public int maxLevel;
    public int baseExp;
    public int currentExp;
    public float levelBuff;

    public float LevelMultiplier
    {
        get
        {
            return 1 + (currentLevel - 1) * levelBuff;
        }
    }

    public void UpdateExp(int point)
    {
        currentExp += point;

        if (currentExp >= baseExp)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        // All methods you want to update your data.

        // Update Level
        currentLevel = Mathf.Clamp(currentLevel + 1, 0, maxLevel);
        baseExp += (int)(baseExp * LevelMultiplier);

        // Update health
        maxHealth = (int)(maxHealth * LevelMultiplier);
        currentHealth = maxHealth;

        Debug.Log("LEVEL UP !!! " + "Current Level: " + currentLevel + " Max health: " + maxHealth);
    }
}
