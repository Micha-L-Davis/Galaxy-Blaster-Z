using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour, IDamageable
{
    GameObject _target;
    [SerializeField]
    Rigidbody _rigidbody;
    [SerializeField]
    float _rotationSpeed;
    [SerializeField]
    float _speed;
    [SerializeField]
    int _damage;

    public int Health => throw new System.NotImplementedException();

    void FixedUpdate()
    {
        if (_target != null)
        {
            Vector3 direction = _target.transform.position - _rigidbody.position;
            direction.Normalize();
            Vector3 rotateAmount = Vector3.Cross(direction, transform.forward);
            _rigidbody.angularVelocity = -rotateAmount * _rotationSpeed;
            _rigidbody.velocity = transform.up * _speed;
        }
    }

    public void SetTarget(GameObject target, int damage)
    {
        _target = target;
        _damage = damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageable i = other.GetComponent<IDamageable>();
        if (i != null)
        {
            Debug.Log(this.gameObject.name + " hit " + other.gameObject.name);
            i.Damage(5);
        }
    }

    public void Damage(int damage)
    {
        Destroy(this.gameObject);
    }
}
