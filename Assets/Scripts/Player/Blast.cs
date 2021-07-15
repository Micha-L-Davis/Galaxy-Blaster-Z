using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blast : MonoBehaviour, IDamageable
{
    [SerializeField]
    float _speed = 16;
    int _health;

    public int Health => _health;

    public void Damage()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        transform.Translate(Vector3.right * _speed * Time.deltaTime);
        if (transform.position.x > 32)
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
