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
    [SerializeField]
    AudioClip _gameOverClip, _levelStartClip, _bossMusicClip, _victoryClip;
    

    private void Start()
    {
        Player.OnPlayerDeath += GameOverAudio;
        _source = GetComponent<AudioSource>();
        BGM();
    }

    public void BGM()
    {
        _source.clip = _levelStartClip;
        _source.Play();
    }

    public void BossMusic()
    {
        _source.clip = _bossMusicClip;
        _source.Play();
    }

    void GameOverAudio()
    {
        _source.clip = _gameOverClip;
        _source.Play();
    }

    void VictoryAudio()
    {
        _source.clip = _victoryClip;
        _source.Play();
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
