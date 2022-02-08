using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    protected static T instance;
    public static T Instance
    {
        get
        {
            if (instance)
            {
                return instance;
            } else
            {
                instance = GameObject.FindObjectOfType<T>();
                if (!instance)
                {
                    // create a new gameobject, add T to it, return T
                    GameObject g = new GameObject();
                    instance = g.AddComponent<T>();
                    instance.hideFlags = HideFlags.HideAndDontSave;
                    return instance;
                }
                return instance;
            }
        }

        private set { }
    }
}

public class SingletonPersistant<T> : Singleton<T> where T : Component
{
    private void Awake()
    {
        if (!instance)
        {
            instance = this as T;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this);
        }
    }
}
