using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private static SpawnManager _instance;

    public static SpawnManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("SpawnManager is NULL");
            return _instance;
        }
    }

    public List<WaveModel> waves;
    public int currentWave;
    bool _stopSpawning;
    [SerializeField]
    GameObject _playerPrefab;
    [SerializeField]
    Player _player;
    [SerializeField]
    GameObject _bigBadBossPrefab;

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
                Instantiate(_bigBadBossPrefab);
                //instantiate final boss
            }
        }
    }

    void SpawnPowerup(GameObject obj)
    {
        Enemy enemy = obj.GetComponent<Enemy>();
        if (enemy.dropsPowerup && _player.Health < 3)
        {
            Instantiate(waves[currentWave].powerupToDrop, enemy.transform.position, Quaternion.identity);
        }
        else
        {
            int r = UnityEngine.Random.Range(1, 4);
            //roll a random number between 1-3

            //instantiate weapon upgrade 1, 2 or 3.
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
    }

    public IEnumerator RespawnPlayerRoutine()
    {
        yield return new WaitForSeconds(2.5f);
        Instantiate(_playerPrefab);
        _stopSpawning = false;
        StartCoroutine(SpawnWaveRoutine());
    }

    private void Awake()
    {
        _instance = this;
    }
}
