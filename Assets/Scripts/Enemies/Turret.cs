using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Enemy
{
    Quaternion _rotation;
    [SerializeField]
    float _rapidfireRate = 0.125f;
    void Start()
    {
        _currentStrength = WeaponStrength.Advanced;
        StartCoroutine(TurretFireRoutine());
    }

    private void Update()
    {
        Vector3 relativePos = _player.transform.position - transform.position;
        _rotation = Quaternion.LookRotation(relativePos, transform.up);
        transform.rotation = _rotation * Quaternion.Euler(0, -90, 0);
    }

    IEnumerator TurretFireRoutine()
    {
        while (!_player.isDead)
        {
            yield return new WaitForSeconds(Random.Range(2f, 4f));
            if (!_concealed)
            {
                for (int i = 0; i <= (int)_currentStrength; i++)
                {
                    var blast = PoolManager.Instance.RequestPoolObject(PoolManager.Instance.enemyBlastPool, PoolManager.Instance.enemyBlastPrefab, PoolManager.Instance.enemyBlastContainer);
                    blast.transform.position = _blastOrigins[0].position;
                    var velocity = _blastOrigins[0].forward * _blastForce;
                    blast.GetComponent<Blast>().FireBlast(velocity);
                    _audio.clip = _blasterClip;
                    _audio.Play();
                    yield return new WaitForSeconds(_rapidfireRate);
                }
            }
        }

    }
}
