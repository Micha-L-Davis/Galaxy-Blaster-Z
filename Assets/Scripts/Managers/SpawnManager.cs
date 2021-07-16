using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    List<WaveModel> _waves;
    WaitWhile _waveEliminatedWait;
    int _currentWave;
    bool _stopSpawning;

    private void Start()
    {
        StartCoroutine(SpawnWaveRoutine());
    }

    IEnumerator SpawnWaveRoutine()
    {
        //come back to this later when we have a UI Manager to display the wave number
        //yield return UI.WaveDisplayRoutine();
        while (!_stopSpawning)
        {
            Debug.Log("Spawning " + _waves.Count + " waves.");
            yield return _waves[_currentWave].LaunchInterval();
            Debug.Log(_waves[_currentWave].name + " launched.");
            yield return new WaitForSeconds(.5f);
            yield return new WaitWhile(_waves[_currentWave].EnemyisAlive);
            Debug.Log("Wave Eliminated.");
            _currentWave++;
            Debug.Log("Next wave is " + _currentWave);
            if (_currentWave > _waves.Count - 1)
            {
                //Debug.Log("That's not a valid next wave. We're done here.");
                _stopSpawning = true;
            }
        }


    }
}
