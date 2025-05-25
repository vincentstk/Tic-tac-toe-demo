using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hiraishin.ObserverPattern;
using UnityEngine.Serialization;

public class AudioSystem : MonoBehaviour
{
    #region Component Configs

    [SerializeField]
    private GameObject audioObject;

    [SerializeField]
    private List<SoundData> SoundDataList;

    [Range(0f, 1f), SerializeField]
    private float globalMusicVolume;
    [Range(0f, 1f), SerializeField]
    private float globalSoundDataVolume;

    private Dictionary<string, SoundData> SoundDataDictionary;
    private SoundData currentBGM;

    private Action<object> OnMusicVolumeChanged;
    private Action<object> OnSoundDataVolumeChanged;
    #endregion

    /// <summary>
    /// Only call after Awake method
    /// </summary>
    public void Init(GameObject otherParent = null)
    {
        if (otherParent != null)
        {
            audioObject = otherParent;
        }
        SoundDataDictionary = new Dictionary<string, SoundData>();
        globalMusicVolume = PlayerPrefs.GetFloat(ConstantDefinitions.MUSIC_VOLUME, 1);
        globalSoundDataVolume = PlayerPrefs.GetFloat(ConstantDefinitions.SFX_VOLUME, 1);
        OnMusicVolumeChanged = (param) => MusicVolumeChanged((VolumeMessage)param);
        OnSoundDataVolumeChanged = (param) => SoundDataVolumeChanged((VolumeMessage)param);
        foreach (SoundData soundData in SoundDataList)
        {
            soundData.audioSource = audioObject.AddComponent<AudioSource>();
            soundData.audioSource.clip = soundData.audioFile;
            soundData.audioSource.outputAudioMixerGroup = soundData.mixer;
            soundData.audioSource.loop = soundData.isLoop;
            soundData.audioSource.playOnAwake = soundData.playOnAwake;
            soundData.audioSource.volume = soundData.type == GameAudioType.Music ? globalMusicVolume : globalSoundDataVolume;
            soundData.audioSource.priority = soundData.priority;
            soundData.audioSource.pitch = soundData.pitch;
            SoundDataDictionary.Add(soundData.name, soundData);
            if (soundData.playOnAwake)
            {
                PlayAudio(soundData.name);
            }
        }
        this.RegisterListener(EventID.ChangeMusicVolume, OnMusicVolumeChanged);
        this.RegisterListener(EventID.ChangeSfxVolume, OnSoundDataVolumeChanged);
    }
    private void MusicVolumeChanged(VolumeMessage message)
    {
        globalMusicVolume = message.volume;
        List<SoundData> allSoundData = SoundDataList.FindAll(s => s.type == GameAudioType.Music);
        if (globalMusicVolume == 0)
        {
            for (int i = 0; i < allSoundData.Count; i++)
            {
                if (!allSoundData[i].audioSource.isPlaying)
                {
                    continue;
                }
                PauseAudio(allSoundData[i].name);
            }
            return;
        }

        for (int i = 0; i < allSoundData.Count; i++)
        {
            allSoundData[i].volume = globalMusicVolume;
            allSoundData[i].audioSource.volume = message.volume;
            if (allSoundData[i].name.Equals(currentBGM.name, StringComparison.OrdinalIgnoreCase) && !allSoundData[i].audioSource.isPlaying)
            {
                PlayAudio(allSoundData[i].name);
            }
        }
    }
    private void SoundDataVolumeChanged(VolumeMessage message)
    {
        globalSoundDataVolume = message.volume;
        List<SoundData> allSoundData = SoundDataList.FindAll(s => s.type == GameAudioType.Sfx);
        if (globalSoundDataVolume == 0)
        {
            for (int i = 0; i < allSoundData.Count; i++)
            {
                if (!allSoundData[i].audioSource.isPlaying)
                {
                    continue;
                }
                StopAudio(allSoundData[i].name);
            }
            return;
        }

        for (int i = 0; i < allSoundData.Count; i++)
        {
            allSoundData[i].volume = globalSoundDataVolume;
            allSoundData[i].audioSource.volume = message.volume;
        }
    }

    public void PlayAudio(string audioName)
    {
        if (!SoundDataDictionary.ContainsKey(audioName))
        {
            return;
        }

        SoundData SoundData = SoundDataDictionary[audioName];
        if (SoundData.type == GameAudioType.Music)
        {
            if (globalMusicVolume == 0)
            {
                return;
            }

            currentBGM = SoundData;
        }
        else if (SoundData.type == GameAudioType.Sfx)
        {
            if (globalSoundDataVolume == 0)
            {
                return;
            }
        }
        SoundData.audioSource.Play();
    }

    public void PauseAudio(string audioName)
    {
        if (!SoundDataDictionary.ContainsKey(audioName))
        {
            return;
        }
        SoundData SoundData = SoundDataDictionary[audioName];
        if (!SoundData.audioSource.isPlaying)
        {
            return;
        }
        SoundData.audioSource.Pause();
    }

    public void StopAudio(string audioName)
    {
        if (!SoundDataDictionary.ContainsKey(audioName))
        {
            return;
        }
        SoundData SoundData = SoundDataDictionary[audioName];
        if (!SoundData.audioSource.isPlaying)
        {
            return;
        }
        SoundData.audioSource.Stop();
    }
}
