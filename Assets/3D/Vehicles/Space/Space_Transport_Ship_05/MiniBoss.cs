using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBoss : Enemy
{
    public void FireVolley()
    {
        foreach (var item in _blastOrigins)
        {
            var blast = PoolManager.Instance.RequestPoolObject(PoolManager.Instance.enemyBlastPool, PoolManager.Instance.enemyBlastPrefab, PoolManager.Instance.enemyBlastContainer);
            blast.transform.position = item.position;
            var velocity = item.forward * _blastForce;
            blast.GetComponent<Blast>().FireBlast(velocity);
            _audio.clip = _blasterClip;
            _audio.Play();
        }
    }
}
