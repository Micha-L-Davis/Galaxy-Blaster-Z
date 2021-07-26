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
    [SerializeField]
    AudioSource _bgmSource;
    [SerializeField]
    AudioSource _sfxSource;
    [SerializeField]
    AudioClip _gameOverClip, _levelStartClip, _bossMusicClip, _victoryClip;
    

    private void Start()
    {
        Player.OnPlayerDeath += GameOverAudio;
        BGM();
    }

    public void BGM()
    {
        _bgmSource.clip = _levelStartClip;
        _bgmSource.Play();
    }

    public void BossMusic()
    {
        _bgmSource.clip = _bossMusicClip;
        _bgmSource.Play();
    }

    void GameOverAudio()
    {
        _bgmSource.clip = _gameOverClip;
        _bgmSource.Play();
    }

    void VictoryAudio()
    {
        _bgmSource.clip = _victoryClip;
        _bgmSource.Play();
    }

    public void PlayAudio(AudioClip clip)
    {
        _sfxSource.clip = clip;
        _sfxSource.Play();
    }

    private void Awake()
    {
        _instance = this;
    }

}
