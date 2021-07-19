using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blast : MonoBehaviour, IDamageable
{
    Vector3 _velocity;
    int _health;
    [SerializeField]
    Rigidbody _rigidbody;
    [SerializeField]
    int _damage;
    [SerializeField]
    bool _isEnemyBlast;
    [SerializeField]
    GameObject _hitEffect;

    public int Health => _health;

    private void Start()
    {
        switch (PlayerPrefs.GetInt("Difficulty", 1))
        {
            case 1:
                if (_isEnemyBlast)
                {
                    _damage = 1;
                }
                else
                {
                    _damage = 3;
                }
                break;
            case 2:
                if (_isEnemyBlast)
                {
                    _damage = 2;
                }
                else
                {
                    _damage = 2;
                }
                break;
            case 3:
                if (_isEnemyBlast)
                {
                    _damage = 3;
                }
                else
                {
                    _damage = 1;
                }
                break;
            default:
                break;
        }
    }

    public void Damage(int damage)
    {
        gameObject.SetActive(false);
    }


    public void FireBlast(Vector3 velocity)
    {
        _velocity = velocity;
    }

    private void Update()
    {
        _rigidbody.velocity = _velocity;
        if (transform.position.x > 32 || transform.position.x < -36 || transform.position.y > 20 || transform.position.y < -20)
            gameObject.SetActive(false);
    }


    private void OnTriggerEnter(Collider other)
    {
        IDamageable i = other.GetComponent<IDamageable>();
        if (i != null)
        {
            Debug.Log(this.gameObject.name + " hit " + other.gameObject.name);
            i.Damage(_damage);
            if (other.GetComponent<Blast>() == null)
                Instantiate(_hitEffect, new Vector3(other.transform.position.x, other.transform.position.y, other.transform.position.z -4), Quaternion.identity);
        }
    }
}
