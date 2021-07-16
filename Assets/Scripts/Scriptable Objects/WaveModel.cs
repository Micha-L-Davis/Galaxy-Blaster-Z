using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Wave", menuName = "Wave")]
public class WaveModel : ScriptableObject
{
    [Header ("Required")]
    [SerializeField]
    int _waveNumber;
    [SerializeField]
    List<GameObject> _members;
    [Header ("Optional")]
    [SerializeField]
    public GameObject powerupToDrop;
    [SerializeField]
    int _powerUpEnemyPattern;
    List<GameObject> _spawned;
    float _intervalDuration;
    

    public IEnumerator LaunchInterval()
    {
        _spawned = new List<GameObject>();
        //Debug.Log("Listening for Enemy Deaths");
        Enemy.OnEnemyDeath += RemoveMember;
        foreach (GameObject obj in _members)
        {
            _intervalDuration = Random.Range(.5f, 1f);
            GameObject enemy = Instantiate(obj);
            _spawned.Add(enemy);
            Enemy e = enemy.GetComponent<Enemy>();
            if (e.dropsPowerup)
            {
                e.anim.SetInteger("Pattern", _powerUpEnemyPattern);
            }
            //Debug.Log("Object spawned. Waiting " + _intervalDuration + " seconds for next spawn.");
            //Debug.Log("There are " + _spawned.Count + " enemies spawned");
            yield return new WaitForSeconds(_intervalDuration);
        }
    }

    void RemoveMember(GameObject obj)
    {
        _spawned.Remove(obj);
        //Debug.Log("Enemy removed. There are " + _spawned.Count + " remaining in this wave");
    }

    public bool EnemyisAlive()
    {
        //Debug.Log("Checking the list");
        return _spawned.Count > 0;
    }
}
