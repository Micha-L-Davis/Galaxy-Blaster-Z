using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Wave", menuName = "Wave")]
public class WaveModel : ScriptableObject
{
    [SerializeField]
    List<GameObject> _members;
    WaitForSeconds _intervalWait;
    [SerializeField]
    float _intervalDuration;

    void LaunchInterval()
    {
        //make this a coroutine, trigger the coroutine from the spawn manager
        foreach (GameObject obj in _members)
        {
            _intervalDuration = Random.Range(.5f, 1f);
            //instantiate the obj
            //yield return _intervalWait;
        }
    }

    void RemoveMember(GameObject obj)
    {
        _members.Remove(obj);
    }

    void Awake()
    {
        _intervalWait = new WaitForSeconds(_intervalDuration);
        Enemy.OnEnemyDeath += RemoveMember;
        SpawnManager.OnWaveStart += LaunchInterval;
    }
}
