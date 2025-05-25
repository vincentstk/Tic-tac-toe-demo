using UnityEngine;

public class GlobalAudioManager : MonoBehaviour
{
    #region Component Configs

    private AudioSystem _audioSystem;

    #endregion
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _audioSystem = gameObject.GetComponent<AudioSystem>();
        _audioSystem.Init();
    }
}
