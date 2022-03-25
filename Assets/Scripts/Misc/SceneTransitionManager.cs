using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// SceneTransitionManager is a little helpful class that plays an animation before transitioning
// to the next scene.
//
// SceneTransitionManager was was written by George Whitfield in Fall of 2021 for a previous game project,
// called "Lingua Litis"
public class SceneTransitionManager : Singleton<SceneTransitionManager>
{
    public Animator sceneTransitionAnimator;


    public void TransitionToScene(string sceneName)
    {
        StartCoroutine("PlaySceneTransitionAnimation", sceneName);
    }

    public void TransitionToSceneInstant(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }

    IEnumerator PlaySceneTransitionAnimation(string s)
    {
        if (sceneTransitionAnimator == null)
        {
            SceneManager.LoadScene(s);
        }
        else
        {
            // play the animation
            sceneTransitionAnimator.SetTrigger("Close");

            // wait for the animatino clip info to be ready
            while (sceneTransitionAnimator.GetCurrentAnimatorClipInfo(0).Length == 0)
            {
                yield return null;
            }

            // wait for the clip to complete before transitioning to the next scene
            float animTime = sceneTransitionAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
            float t = 0;
            while (t < animTime + 0.25f) // wait an extra quater-second
            {
                t += Time.deltaTime;
                yield return null;
            }

            SceneManager.LoadScene(s);
        }
    }
}
