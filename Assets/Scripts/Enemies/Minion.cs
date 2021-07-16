using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : Enemy
{
    void Start()
    {
        StartCoroutine(EnemyFireRoutine());
    }
    IEnumerator EnemyFireRoutine()
    {
        while (!_player.isDead)
        {
            yield return new WaitForSeconds(Random.Range(2f, 4f));

            foreach (var item in _blastOrigins)
            {
                var blast = PoolManager.Instance.RequestPoolObject(PoolManager.Instance.enemyBlastPool, PoolManager.Instance.enemyBlastPrefab, PoolManager.Instance.enemyBlastContainer);
                blast.transform.position = item.position;
                var direction = (_player.transform.position - transform.position);
                var velocity = direction * _blastForce;
                blast.GetComponent<Blast>().FireBlast(velocity);
            }

        }
    }

}
