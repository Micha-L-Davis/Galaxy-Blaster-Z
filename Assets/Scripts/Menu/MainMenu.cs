using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    GameObject _settingsCanvas;
    [SerializeField]
    AudioSource _uiAudio;
    [SerializeField]
    AudioClip _buttonClip, _closeClip, _scrollClip;

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Options()
    {
        _settingsCanvas.SetActive(true);
    }

    public void CloseOptions()
    {
        _settingsCanvas.SetActive(false);
    }

    public void ButtonSFX()
    {
        _uiAudio.clip = _buttonClip;
        _uiAudio.Play();
    }

    public void CloseFrameSFX()
    {
        _uiAudio.clip = _closeClip;
        _uiAudio.Play();
    }

    public void ScrollSFX()
    {
        _uiAudio.clip = _scrollClip;
        _uiAudio.Play();
    }
}
