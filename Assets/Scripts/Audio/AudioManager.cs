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

    private float timer = 0.0f;

    private void Awake()
    {
        if (!Instance) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        AmbientAudio();
    }

    private void playAudio(EventReference audio, GameObject gb)
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
        playAudio(shootAudio, gb);
    }

    public void FootstepAudio(GameObject gb, Vector3 movement, float moveSpeed)
    {
        if (movement != Vector3.zero)
        {
            if (timer > 1)
            {
                playAudio(footStepAudio, gb);
                timer = 0.0f;
            }
            timer += Time.deltaTime * moveSpeed;
        }
    }

    public void AmbientAudio()
    {
        playAudio(ambientAudio, gameObject);
    }
}