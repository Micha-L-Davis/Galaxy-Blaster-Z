using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blast : MonoBehaviour, IDamageable
{
    [SerializeField]
    Vector3 _velocity;
    int _health;
    [SerializeField]
    Rigidbody _rigidbody;

    public int Health => _health;

    public void Damage()
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
        if (transform.position.x > 32 || transform.position.x < -36)
            gameObject.SetActive(false);
    }


    private void OnTriggerEnter(Collider other)
    {
        IDamageable i = other.GetComponent<IDamageable>();
        if (i != null)
        {
            i.Damage();
        }
    }
}
