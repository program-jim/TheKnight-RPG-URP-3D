using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ScriptableObject which stores data of attack.
/// </summary>
[CreateAssetMenu(fileName = "New Attack Data", menuName = "Attack/Attack Data")]
public class AttackData_SO : ScriptableObject
{
    public float attackRange;
    public float skillRange;
    public float coolDown;
    public int maxDamage;
    public int minDamage;

    public float criticalMultiplier;
    public float criticalChance;
}
