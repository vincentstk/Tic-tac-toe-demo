using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

[System.Serializable]
public class SoundData
{
    public string name;
    public GameAudioType type;
    public AudioClip audioFile;
    public AudioMixerGroup mixer;
    public bool isLoop;
    public bool playOnAwake;
    [Range(0f, 1f)]
    public float volume = 1;
    [Range(0, 256)]
    public int priority = 128;
    [Range(-3f, 3f)]
    public float pitch = 1;

    [HideInInspector]
    public AudioSource audioSource;
}

public enum GameAudioType : byte
{
    Sfx,
    Music
}
