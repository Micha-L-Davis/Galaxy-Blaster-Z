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
    [SerializeField]
    List<GameObject> _powerUpPrefabs;
    [SerializeField]
    int _checkPointWave = 7;


    private void Start()
    {
        Enemy.OnEnemyDeath += SpawnPowerup;
        Player.OnPlayerDeath += Respawn;
        StartCoroutine(SpawnWaveRoutine());
    }

    IEnumerator SpawnWaveRoutine()
    {
        yield return UIManager.Instance.WaveUpdateRoutine(currentWave);
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
                _stopSpawning = true;
                Instantiate(_bigBadBossPrefab);
                AudioManager.Instance.BossMusic();
            }
            else
                yield return UIManager.Instance.WaveUpdateRoutine(currentWave);
        }
    }

    void SpawnPowerup(GameObject obj)
    {
        Enemy enemy = obj.GetComponent<Enemy>();
        if (enemy.dropsPowerup)
        {
            if (_player.Health < 3)
            {
                Instantiate(_powerUpPrefabs[0], obj.transform.position, Quaternion.identity);
            }
            else
            {
                int r = UnityEngine.Random.Range(0, 3);
                Instantiate(_powerUpPrefabs[r], obj.transform.position, Quaternion.identity);
            }
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
        if (currentWave > _checkPointWave)
            currentWave = _checkPointWave + 1;
        else if (currentWave > waves.Count - 1)
        {
            _stopSpawning = true;
            Instantiate(_bigBadBossPrefab);
            AudioManager.Instance.BossMusic();
        }
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
