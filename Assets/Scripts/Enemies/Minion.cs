using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : Enemy
{
    void Start()
    {
        StartCoroutine(MinionFireRoutine());
    }
    IEnumerator MinionFireRoutine()
    {
        while (!_player.isDead)
        {
            yield return new WaitForSeconds(Random.Range(2f, 4f));
            if (!_concealed)
            {
                foreach (var item in _blastOrigins)
                {
                    var blast = PoolManager.Instance.RequestPoolObject(PoolManager.Instance.enemyBlastPool, PoolManager.Instance.enemyBlastPrefab, PoolManager.Instance.enemyBlastContainer);
                    blast.transform.position = item.position;
                    item.transform.LookAt(_player.transform.position);
                    var velocity = item.forward * _blastForce;
                    blast.GetComponent<Blast>().FireBlast(velocity);
                    _audio.clip = _blasterClip;
                    _audio.Play();
                }
            }
        }
    }

}
