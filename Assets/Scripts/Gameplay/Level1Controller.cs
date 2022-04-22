using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Controller : Singleton<Level1Controller>
{
    public string nextScene;

    // this function is called when the level is completed
    public void OnLevelCompleted()
    {
        SceneTransitionManager.Instance.TransitionToScene(nextScene);
    }
}
