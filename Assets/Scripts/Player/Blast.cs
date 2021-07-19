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
    float _rotationSpeed = 30;

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
        //Vector3 desiredRotation = Quaternion.LookRotation(_rigidbody.velocity, transform.up).eulerAngles;
        //Vector3 rotation = desiredRotation - _rigidbody.rotation.eulerAngles;
        //rotation.Normalize();
        //rotation *= _rotationSpeed;
        //_rigidbody.AddTorque(rotation);
        if (transform.position.x > 32 || transform.position.x < -36 || transform.position.y > 20 || transform.position.y < -20)
            gameObject.SetActive(false);
    }


    private void OnTriggerEnter(Collider other)
    {
        IDamageable i = other.GetComponent<IDamageable>();
        if (i != null)
        {
            Debug.Log(this.gameObject.name + " hit " + other.gameObject.name);
            i.Damage();
        }
    }
}
