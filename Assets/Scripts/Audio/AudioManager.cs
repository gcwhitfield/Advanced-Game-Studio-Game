using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public EventReference shootAudio;
    public EventReference footStepAudio;

    private float timer = 0.0f;

    private void Awake()
    {
        if (!Instance) Instance = this as AudioManager;
    }

    public void ShootAudio(GameObject gb)
    {
        FMOD.Studio.EventInstance shootInstance;
        shootInstance = RuntimeManager.CreateInstance(shootAudio);
        RuntimeManager.AttachInstanceToGameObject(shootInstance, gb.transform);

        shootInstance.start();
        shootInstance.release();
    }

    public void FootstepAudio(GameObject gb, Vector3 movement, float moveSpeed)
    {
        FMOD.Studio.EventInstance footStepInstance;
        footStepInstance = RuntimeManager.CreateInstance(footStepAudio);
        RuntimeManager.AttachInstanceToGameObject(footStepInstance, gb.transform);
        //footStepInstance.set3DAttributes(RuntimeUtils.To3DAttributes(gb.transform.position));
        if (movement != Vector3.zero)
        {
            if (timer > moveSpeed / 12)
            {
                footStepInstance.start();
                footStepInstance.release();
                timer = 0.0f;
            }
            timer += Time.deltaTime;
        }
    }
}