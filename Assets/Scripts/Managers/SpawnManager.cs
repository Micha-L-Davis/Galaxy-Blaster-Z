using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public List<WaveModel> waves;
    WaitWhile _waveEliminatedWait;
    public int currentWave;
    bool _stopSpawning;
    [SerializeField]
    GameObject _playerPrefab;

    private void Start()
    {
        Enemy.OnEnemyDeath += SpawnPowerup;
        Player.OnPlayerDeath += Respawn;
        StartCoroutine(SpawnWaveRoutine());
    }

    IEnumerator SpawnWaveRoutine()
    {
        //come back to this later when we have a UI Manager to display the wave number
        //yield return UI.WaveDisplayRoutine();
        while (!_stopSpawning)
        {
            Debug.Log("Spawning " + waves.Count + " waves.");
            yield return waves[currentWave].LaunchInterval();
            Debug.Log(waves[currentWave].name + " launched.");
            yield return new WaitForSeconds(.5f);
            yield return new WaitWhile(waves[currentWave].EnemyisAlive);
            Debug.Log("Wave Eliminated.");
            currentWave++;
            Debug.Log("Next wave is " + currentWave);
            if (currentWave > waves.Count - 1)
            {
                //Debug.Log("That's not a valid next wave. We're done here.");
                _stopSpawning = true;

                //instantiate final boss
            }
        }
    }

    void SpawnPowerup(GameObject obj)
    {
        Enemy enemy = obj.GetComponent<Enemy>();
        if (enemy.dropsPowerup)
        {
            Instantiate(waves[currentWave].powerupToDrop, enemy.transform.position, Quaternion.identity);
        }
    }

    void Respawn()
    {
        _stopSpawning = true;
        StopAllCoroutines();
        StopCoroutine(waves[currentWave].LaunchInterval());
        foreach (var item in waves[currentWave].spawned)
        {
            Destroy(item.gameObject, 0.25f);
        }
        waves[currentWave].spawned.Clear();
        if (currentWave > 8)
            currentWave = 9;
        else
            currentWave = 0;

        StartCoroutine(RespawnPlayerRoutine());
    }

    private IEnumerator RespawnPlayerRoutine()
    {
        yield return new WaitForSeconds(2.5f);
        Instantiate(_playerPrefab);
        _stopSpawning = false;
        StartCoroutine(SpawnWaveRoutine());
    }
}
