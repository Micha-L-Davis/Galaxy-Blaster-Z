using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable, IConcealable
{
    public static Action<GameObject> OnEnemyDeath;
    [SerializeField]
    protected int _health;
    [SerializeField]
    protected bool _concealed;
    [SerializeField]
    protected Player _player;
    [SerializeField]
    protected List<Transform> _blastOrigins;
    [SerializeField]
    protected float _blastForce;
    [SerializeField]
    public bool dropsPowerup;
    [SerializeField]
    public Animator anim;
    [SerializeField]
    SpawnManager _spawnManager;
    [SerializeField]
    List<Animator> _explosionAnimators;
    [SerializeField]
    protected AudioClip _explosionClip, _blasterClip;
    [SerializeField]
    protected AudioSource _audio;
    [SerializeField]
    Collider _collider;


    protected enum WeaponStrength
    {
        Basic,
        Intermediate,
        Advanced
    };
    [SerializeField]
    protected WeaponStrength _currentStrength = WeaponStrength.Basic;

    public int Health => _health;

    public void ToggleConcealment()
    {
        if (!_concealed)
            _concealed = true;
        else
            _concealed = false;
    }

    public void Damage()
    {
        if (_concealed)
        {
            return;
        }
        _health--;
        if (Health < 1)
        {
            _collider.enabled = false;
            foreach (var anim in _explosionAnimators)
            {
                anim.SetTrigger("Explode");
            }
            _audio.clip = _explosionClip;
            _audio.Play();
            if (OnEnemyDeath != null)
            {
                OnEnemyDeath(this.gameObject);
            }
            SpawnPowerup();
            Destroy(this.gameObject, 0.3f);
        }
    }

    void SpawnPowerup()
    {
        if (dropsPowerup)
        {
            Instantiate(_spawnManager.waves[_spawnManager.currentWave].powerupToDrop, transform.position, Quaternion.identity);
        }
    }

    protected void OnTriggerEnter(Collider other)
    {
        IDamageable i = other.GetComponent<IDamageable>();
        if (i != null)
        {
            i.Damage();
        }
    }
}
