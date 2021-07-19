using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Wave", menuName = "Wave")]
public class WaveModel : ScriptableObject
{
    [Header("Required")]
    [SerializeField]
    List<GameObject> _members;
    [SerializeField]
    int waveFlightPattern;
    [Header("Optional")]
    public List<GameObject> spawned;
    float _intervalDuration;

    public IEnumerator LaunchInterval()
    {
        spawned = new List<GameObject>();
        //Debug.Log("Listening for Enemy Deaths");
        Enemy.OnEnemyDeath += RemoveMember;
        foreach (GameObject obj in _members)
        {
            _intervalDuration = Random.Range(.5f, 1f);
            GameObject enemy = Instantiate(obj);
            spawned.Add(enemy);
            Enemy e = enemy.GetComponent<Enemy>();
            e.anim.SetInteger("Pattern", waveFlightPattern);
            //Debug.Log("Object spawned. Waiting " + _intervalDuration + " seconds for next spawn.");
            //Debug.Log("There are " + _spawned.Count + " enemies spawned");
            yield return new WaitForSeconds(_intervalDuration);
        }
    }

    void RemoveMember(GameObject obj)
    {
        spawned.Remove(obj);
        //Debug.Log("Enemy removed. There are " + _spawned.Count + " remaining in this wave");
    }

    public bool EnemyisAlive()
    {
        //Debug.Log("Checking the list");
        return spawned.Count > 0;
    }
}
