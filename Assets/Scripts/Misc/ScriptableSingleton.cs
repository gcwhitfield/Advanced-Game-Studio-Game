using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// singleton scriptable object code taken from YouTube video explaining
// singleton scriptable objects. https://www.youtube.com/watch?v=cH-QQoNNpaI
public abstract class ScriptableSingleton<T> : ScriptableObject where T : ScriptableObject
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (!instance)
            {
                T[] results = Resources.FindObjectsOfTypeAll<T>();
                if (results.Length == 0)
                {
                    Debug.LogError("Could not find scriptable singleton");
                    return null;
                } else if (results.Length > 1)
                {
                    Debug.LogError("Multiple scriptable singletons of same type detected. There can only" +
                        "be one instance of a scrtipable singleton");
                    return null;
                } else
                {
                    instance = results[0];
                    instance.hideFlags = HideFlags.DontUnloadUnusedAsset;
                }
            }
            return instance;
        }
    }
}