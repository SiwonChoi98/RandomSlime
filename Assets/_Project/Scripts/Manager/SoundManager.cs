using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class BGMStruct
{
    public string name;
    public int index;
    public AudioClip clip;
}
[Serializable]
public class SFXStruct
{
    public string name;
    public int index;
    public AudioClip clip;
}
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioSource bgmAudioSource;
    public AudioSource sfxAudioSource;
    public List<BGMStruct> bgmSoundList;
    public List<SFXStruct> sfxSoundList;
    void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != null) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    public void BgmPlaySound(string name, float volume = 1f)
    {
        int soundIndex = bgmSoundList.Find(t => t.name == name).index;
        bgmAudioSource.clip = bgmSoundList[soundIndex].clip;
        bgmAudioSource.volume = volume;
        bgmAudioSource.Play();

    }
    public void SfxPlaySound(string name, float volume = 1f)
    {
        int soundIndex = sfxSoundList.Find(t => t.name == name).index;
        sfxAudioSource.clip = sfxSoundList[soundIndex].clip;
        
        sfxAudioSource.PlayOneShot(sfxAudioSource.clip, volume);
    }

}