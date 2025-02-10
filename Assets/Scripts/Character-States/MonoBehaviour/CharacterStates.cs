using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MonoBehaviour which shows data of a character's states.
/// </summary>
public class CharacterStates : MonoBehaviour
{
    public CharacterData_SO templateData;
    [HideInInspector] public CharacterData_SO characterData;
    public AttackData_SO attackData;
    [HideInInspector] public bool isCritical;

    private void Awake()
    {
        if (templateData != null)
        {
            characterData = Instantiate(templateData);
        }
    }

    #region Read from CharacterData_SO File

    public int MaxHealth
    {
        get
        {
            if (characterData != null)
            {
                return characterData.maxHealth;
            }
            else
            {
                return 0;
            }
        }
        set
        {
            characterData.maxHealth = value;
        }
    }

    public int CurrentHealth
    {
        get
        {
            if (characterData != null)
            {
                return characterData.currentHealth;
            }
            else
            {
                return 0;
            }
        }
        set
        {
            characterData.currentHealth = value;
        }
    }

    public int BaseDefence
    {
        get
        {
            if (characterData != null)
            {
                return characterData.baseDefence;
            }
            else
            {
                return 0;
            }
        }
        set
        {
            characterData.baseDefence = value;
        }
    }

    public int CurrentDefence
    {
        get
        {
            if (characterData != null)
            {
                return characterData.currentDefence;
            }
            else
            {
                return 0;
            }
        }
        set
        {
            characterData.currentDefence = value;
        }
    }

    #endregion

    #region Character Combat

    public void TakeDamage(CharacterStates attacker, CharacterStates defener)
    {
        int damage = Mathf.Max(attacker.CurrentDamage() - defener.CurrentDefence, 0);
        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);

        if (attacker.isCritical)
        {
            defener.GetComponent<Animator>().SetTrigger("Hit");
        }
        // TODO: Update UI
        // TODO: Experience Update
    }

    private int CurrentDamage()
    {
        float coreDamage = UnityEngine.Random.Range(attackData.minDamage, attackData.maxDamage);

        if (isCritical)
        {
            coreDamage *= attackData.criticalMultiplier;
            Debug.Log("Critical damage! " + coreDamage + "\nfrom " + gameObject.name);
        }

        return (int)coreDamage;
    }

    #endregion

}
