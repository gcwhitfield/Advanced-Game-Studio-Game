using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3Controller : Singleton<Level3Controller>
{

    public GameObject fatherScreen;
    public GameObject daughterScreen;
    public GameObject dualScreen;
    public Light daughterLight;

    public Interactable clearingEntranceInteractable;
    public Interactable levelCompletedInteractable;

    public string nextLevel;

    // this coroutine is called when the clearing is reached in Level 3
    IEnumerator DoClearingReached()
    {
        // change the camera from single screen to split screen
        {
            // step 1) fade the screen to black, wait for the animation to finish
            SceneTransitionManager.Instance.sceneTransitionAnimator.SetTrigger("Close");
            yield return new WaitForSeconds(0.1f);
            float t = SceneTransitionManager.Instance.sceneTransitionAnimator.GetCurrentAnimatorClipInfo(0).Length;
            while (t > 0)
            {
                t -= Time.deltaTime;
                yield return null;
            }
            // step 2) change screen to split screen
            fatherScreen.SetActive(true);
            daughterScreen.SetActive(true);
            dualScreen.SetActive(false);

            // disable daughter's flashlight
            daughterLight.enabled = false;

            // step 3) fade the screen back in, wait for the animation to finish
            SceneTransitionManager.Instance.sceneTransitionAnimator.SetTrigger("Open");
            yield return new WaitForSeconds(0.1f);
            t = SceneTransitionManager.Instance.sceneTransitionAnimator.GetCurrentAnimatorClipInfo(0).Length;
            while (t > 0)
            {
                t -= Time.deltaTime;
                yield return null;
            }
        }
        yield return null;
    }

    // this function is called when the player collides with the invisible clearing
    // checkpoint interactable in Level 3
    public void OnClearingReached()
    {
        StartCoroutine("DoClearingReached");
    }

    public void OnLevelCompleted()
    {
        SceneTransitionManager.Instance.TransitionToScene(nextLevel);
    }

    // Start is called before the first frame update
    void Start()
    {
        clearingEntranceInteractable.ExecuteOnTriggerEnter(OnClearingReached);
        levelCompletedInteractable.ExecuteOnTriggerEnter(OnLevelCompleted);
    }
}
