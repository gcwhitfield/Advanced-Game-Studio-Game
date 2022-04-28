using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3Controller : Singleton<Level3Controller>
{
    [Header("Father and daughter screens")]
    public GameObject fatherScreen;

    public GameObject daughterScreen;
    public GameObject dualScreen;

    [Header("Gameplay object references")]
    public Light daughterLight;

    public Interactable clearingEntranceInteractable;
    public Interactable levelCompletedInteractable;
    public GameObject keysSparkle;
    public Interactable key1;
    public Interactable key2;
    public Interactable key3;
    public Interactable key4;
    public Collider fenceLockCollider; // the collider that is attached to the fence lock.

    // this collider should initially be DISABLED, which makes the player unable to interact with the
    // fence lock at first. Then, when the player collects all 4 keys, the fence lock will
    // get enabled and they can walk up to it to activate the fence lock UI
    private bool key1Collected = false;

    private bool key2Collected = false;
    private bool key3Collected = false;
    private bool key4Collected = false;

    [Header("Scene Management")]
    public string nextLevel;

    // Start is called before the first frame update
    private void Start()
    {
        clearingEntranceInteractable.ExecuteOnTriggerEnter(OnClearingReached);
        levelCompletedInteractable.ExecuteOnTriggerEnter(OnLevelCompleted);

        // set up interaction events for the key collectables
        key1.ExecuteOnInteract(Key1Collected);
        key2.ExecuteOnInteract(Key2Collected);
        key3.ExecuteOnInteract(Key3Collected);
        key4.ExecuteOnInteract(Key4Collected);
    }

    // called when the first key is collected
    public void Key1Collected()
    {
        key1Collected = true;
    }

    // called when the second key is collected
    public void Key2Collected()
    {
        key2Collected = true;
    }

    // called when the third key is collected
    public void Key3Collected()
    {
        key3Collected = true;
    }

    // called when the final key is collected
    public void Key4Collected()
    {
        key4Collected = true;
    }

    // this coroutine is called when the clearing is reached in Level 3
    private IEnumerator DoClearingReached()
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

            // wait for the player to collect all of the keys
            while (!key1Collected || !key2Collected || !key3Collected || !key4Collected)
            {
                yield return null;
            }

            // active the fence lock. after the fence lock has been activated, the daughter
            // can walk up to the fence lock to use it
            fenceLockCollider.enabled = true;
            keysSparkle.SetActive(true);
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
}