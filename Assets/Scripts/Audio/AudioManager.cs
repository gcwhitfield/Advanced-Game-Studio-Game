using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public string level;

    public GameObject monster;
    public GameObject wolf;

    public EventReference shootAudio;
    public EventReference footStepAudio;
    public EventReference ambientAudio;
    public EventReference lanternWalkingAduio;
    public EventReference wolfShowUp;
    public EventReference wolfBite;
    public EventReference pickUp;
    public EventReference inputCode;
    public EventReference monsterApproach;
    public EventReference bone;
    public EventReference wolfChase;
    public EventReference menuHover;
    public EventReference menuEnter;
    public EventReference menuTheme;
    public EventReference lockCorrect;
    public EventReference lockWrong;
    public EventReference lockHover;
    public EventReference keyCorrect;
    public EventReference keyWrong;
    public EventReference keyPick;
    public EventReference keyRotate;
    public EventReference keyUnlock;
    public EventReference themeEmo;
    public EventReference cutScene;
    public EventReference monsterAttackBeast;
    public EventReference monsterGetBeat;
    public EventReference monsterChase;
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
        FMOD.Studio.Bus MasterBus;
        MasterBus = FMODUnity.RuntimeManager.GetBus("Bus:/");
        MasterBus.stopAllEvents(FMOD.Studio.STOP_MODE.IMMEDIATE);
        if (level.Contains("Menu"))
        {
            MenuThemeAudio();
            return;
        }
        if (level.Contains("CutScene"))
        {
            CutSceneAudio();
            return;
        }
        if (level.Contains("Level 3"))
        {
            // play level 3 chase music
            MonsterChaseAudio();
            return;
        }
        if (level.Contains("Level 2"))
        {
            WolfChaseAudio(wolf);
            MonsterApproachAudio(monster);
            AmbientAudio();
            return;
        }
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

    private void PlayAudioNoRelease(EventReference audio, GameObject gb)
    {
        FMOD.Studio.EventInstance audioInstance;
        audioInstance = RuntimeManager.CreateInstance(audio);
        RuntimeManager.AttachInstanceToGameObject(audioInstance, gb.transform);
        //footStepInstance.set3DAttributes(RuntimeUtils.To3DAttributes(gb.transform.position));
        audioInstance.start();
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

    public void WolfShowUpAudio(GameObject gb)
    {
        PlayAudio(wolfShowUp, gb);
    }

    public void WolfBiteAudio(GameObject gb)
    {
        PlayAudio(wolfBite, gb);
    }

    public void PickUpAudio(GameObject gb)
    {
        PlayAudio(pickUp, gb);
    }

    public void InputCodeAudio(GameObject gb)
    {
        PlayAudio(inputCode, gb);
    }

    public void MonsterApproachAudio(GameObject gb)
    {
        PlayAudio(monsterApproach, gb);
    }

    public void BoneAudio(GameObject gb)
    {
        PlayAudio(bone, gb);
    }

    public void MenuHoverAudio(GameObject gb)
    {
        PlayAudio(menuHover, gb);
    }

    public void MenuEnterAudio(GameObject gb)
    {
        PlayAudio(menuEnter, gb);
    }

    public void LockCorrectAudio(GameObject gb)
    {
        PlayAudio(lockCorrect, gb);
    }

    public void LockWrongAudio(GameObject gb)
    {
        PlayAudio(lockWrong, gb);
    }

    public void LockHoverAudio(GameObject gb)
    {
        PlayAudio(lockHover, gb);
    }

    public void KeyCorrectAudio(GameObject gb)
    {
        PlayAudio(keyCorrect, gb);
    }

    public void KeyWrongAudio(GameObject gb)
    {
        PlayAudio(keyWrong, gb);
    }

    public void KeyUnlockAudio(GameObject gb)
    {
        PlayAudio(keyUnlock, gb);
    }

    public void KeyPickAudio(GameObject gb)
    {
        PlayAudio(keyPick, gb);
    }

    public void KeyRotateAudio(GameObject gb)
    {
        PlayAudio(keyRotate, gb);
    }

    public void MenuThemeAudio()
    {
        PlayAudio(menuTheme, gameObject);
    }

    public void CutSceneAudio()
    {
        PlayAudio(cutScene, gameObject);
    }

    public void ThemeEmoAudio()
    {
        PlayAudio(themeEmo, gameObject);
    }

    public void MonsterAttackBeastAudio(GameObject gb)
    {
        PlayAudio(monsterAttackBeast, gb);
    }

    public void MonsterGetBeatAudio(GameObject gb)
    {
        PlayAudio(monsterGetBeat, gb);
    }

    public void MonsterChaseAudio()
    {
        PlayAudio(monsterChase, gameObject);
    }

    private FMOD.Studio.EventInstance wolfInstance;

    public void WolfChaseAudio(GameObject gb)
    {
        wolfInstance = RuntimeManager.CreateInstance(wolfChase);
        RuntimeManager.AttachInstanceToGameObject(wolfInstance, gb.transform);
        //footStepInstance.set3DAttributes(RuntimeUtils.To3DAttributes(gb.transform.position));
        wolfInstance.start();
        wolfInstance.release();
    }

    public void WolfChaseAudioStop()
    {
        wolfInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }
}