using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This abstract class rappresents a single instanced object.
/// </summary>
/// <typeparam name="T">The object can be of any type</typeparam>
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<T>();
            }

            return instance;
        }
    }
}
