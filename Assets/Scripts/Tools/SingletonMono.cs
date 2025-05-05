using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class which can achieve singleton of a MonoBehaviour.
/// </summary>
/// <typeparam name="T">The template to be singleton.</typeparam>
public class SingletonMono<T> : MonoBehaviour where T : SingletonMono<T>
{
    private static T instance;
    public static T Instance
    {
        get
        {
            return instance;
        }
    }
    public static bool IsInitialized
    {
        get
        {
            return instance != null;
        }
    }

    protected virtual void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = (T)this;
        }
    }

    protected virtual void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}
