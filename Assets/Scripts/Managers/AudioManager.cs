using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;

    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("AudioManager is Null");
            return _instance;
        }
    }

    AudioSource _source;

    private void Start()
    {
        _source = GetComponent<AudioSource>();
    }

    public void PlayAudio(AudioClip clip)
    {
        _source.clip = clip;
        _source.Play();
    }

    private void Awake()
    {
        _instance = this;
    }

}
