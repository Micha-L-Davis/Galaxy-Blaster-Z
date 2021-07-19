using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBadBoss : MonoBehaviour
{
    [SerializeField]
    int _phase = 0;
    [SerializeField]
    List<GameObject> _phase0Turrets, _phase1Turrets, _phase2Turrets;
    List<List<GameObject>> _turretLists = new List<List<GameObject>>();
    [SerializeField]
    Animator _anim;
    [SerializeField]
    GameObject _smokeCloudPrefab;

    void Start()
    {
        Enemy.OnEnemyDeath += RemoveTurret;
        _turretLists.Add(_phase0Turrets);
        _turretLists.Add(_phase1Turrets);
        _turretLists.Add(_phase2Turrets);
        StartCoroutine(LeftRightRoutine());
    }

    void RemoveTurret(GameObject turret)
    {
        GameObject smoke = Instantiate(_smokeCloudPrefab, turret.transform.position, Quaternion.identity, this.transform);
        smoke.transform.localScale = new Vector3(.1f, .1f, .1f);
        _turretLists[_phase].Remove(turret);
        if (_turretLists[_phase].Count == 0)
        {
            _phase++;
            UpdatePhase();
        }

        if (_phase > 2)
        {
            StopAllCoroutines();
            //disable whatever might still be running.
            UIManager.Instance.Victory();
        }

    }


    void UpdatePhase()
    {
        _anim.SetInteger("Phase", _phase);
    }

    IEnumerator LeftRightRoutine()
    {
        var waitTime = new WaitForSeconds(7);
        yield return waitTime;
        while (_phase < 3)
        {
            yield return waitTime;
            _anim.SetTrigger("CrossToLeft");
            //RotationCorrection();
            yield return waitTime;
            _anim.SetTrigger("CrossToRight");
        }

    }

}
