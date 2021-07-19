using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetDifficulty : MonoBehaviour
{
    [SerializeField]
    Toggle _easyToggle, _normalToggle, _hardToggle;

    private void Start()
    {
        switch (PlayerPrefs.GetInt("Difficulty"))
        {
            case 1:
                _easyToggle.isOn = true;
                break;
            case 2:
                _normalToggle.isOn = true;
                break;
            case 3:
                _hardToggle.isOn = true;
                break;
            default:
                break;
        }
    }

    public void ToggleEasy()
    {
        PlayerPrefs.SetInt("Difficulty", 1);
    }

    public void ToggleNormal()
    {
        PlayerPrefs.SetInt("Difficulty", 2);
    }

    public void ToggleHard()
    {
        PlayerPrefs.SetInt("Difficulty", 3);
    }
}

