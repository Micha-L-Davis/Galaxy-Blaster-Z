using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;

    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("UI Manager is NULL");
            return _instance;
        }
    }

    [SerializeField]
    Text _scoreText;
    [SerializeField]
    List<Image> _weaponRankImages;
    [SerializeField]
    List<Image> _currentWeaponImage;
    [SerializeField]
    List<GameObject> _canvasPanels; //0 - Pause, 1 - Game Over, 2 - Victory
    [SerializeField]
    GameObject _waveIndicator;
    [SerializeField]
    Text _waveText;
    
    public void StrengthUpdate(int strength)
    {
        if (strength <= -1)
        {
            _weaponRankImages[0].gameObject.SetActive(false);
            _canvasPanels[1].gameObject.SetActive(true);
            return;
        }

        if (!_weaponRankImages[strength].IsActive())
        {
            _weaponRankImages[strength].gameObject.SetActive(true);
            return;
        }

        if (_weaponRankImages[strength+1].IsActive())
        {
            _weaponRankImages[strength+1].gameObject.SetActive(false);
        }
    }

    public void ScoreUpdate(int score)
    {
        _scoreText.text = "" + score;
    }

    public void WeaponUpdate(int weapon)
    {

    }

    public IEnumerator WaveUpdate(int waveNumber)
    {
        _waveIndicator.SetActive(true);
        _waveText.text = "WAVE " + waveNumber + " INCOMING!";
        yield return new WaitForSeconds(2.5f);
    }

    public void Restart()
    {
        StartCoroutine(SpawnManager.Instance.RespawnPlayerRoutine());
        foreach (var item in _canvasPanels)
        {
            item.gameObject.SetActive(false);
        }
        
        
    }

    public void Pause()
    {
        _canvasPanels[0].gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    public void Resume()
    {
        _canvasPanels[0].gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void Quit()
    {
        SceneManager.LoadScene(0);
    }

    public void Victory()
    {
        _canvasPanels[2].gameObject.SetActive(true);
    }

    private void Awake()
    {
        _instance = this;
    }
}


