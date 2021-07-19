using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    GameObject _settingsCanvas;
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
}
