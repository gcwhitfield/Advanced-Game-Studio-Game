using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public EventReference shootAudio;
    public EventReference footStepAudio;
    public EventReference ambientAudio;
    public EventReference themeAudio;
    public EventReference lanternWalkingAduio;
    //public EventReference lanternLaunchAduio;

    private float footstepTimer = 0.0f;
    private float lanternTimer = 0.0f;

    private void Awake()
    {
        if (!Instance) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        AmbientAudio();
    }

    private void PlayAudio(EventReference audio, GameObject gb)
    {
        FMOD.Studio.EventInstance audioInstance;
        audioInstance = RuntimeManager.CreateInstance(audio);
        RuntimeManager.AttachInstanceToGameObject(audioInstance, gb.transform);
        //footStepInstance.set3DAttributes(RuntimeUtils.To3DAttributes(gb.transform.position));
        audioInstance.start();
        audioInstance.release();
    }

    public void ShootAudio(GameObject gb)
    {
        PlayAudio(shootAudio, gb);
    }

    public void FootstepAudio(GameObject gb, Vector3 movement, float moveSpeed)
    {
        if (movement != Vector3.zero)
        {
            if (footstepTimer > 1)
            {
                PlayAudio(footStepAudio, gb);
                footstepTimer = 0.0f;
            }
            footstepTimer += Time.deltaTime * moveSpeed;
        }
    }

    public void AmbientAudio()
    {
        PlayAudio(ambientAudio, gameObject);
    }

    public void LanternWalkingAudio(GameObject gb, Vector3 movement)
    {
        if (movement != Vector3.zero)
        {
            if (lanternTimer > 30)
            {
                PlayAudio(lanternWalkingAduio, gb);
                lanternTimer = 0.0f;
            }
            lanternTimer += Time.deltaTime * Random.Range(0f, 10.0f);
        }
    }

    public void LanternLanuchAudio()
    {
    }
}