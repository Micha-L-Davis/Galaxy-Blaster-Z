using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blast : MonoBehaviour, IDamageable
{
    [SerializeField]
    float _speed = 8;
    int _health = 1;

    public int Health => _health;

    public void Damage()
    {
        Destroy(this);
    }

    private void Update()
    {
        transform.Translate(Vector3.right * _speed * Time.deltaTime);
        if (transform.position.x > 32)
            gameObject.SetActive(false);
    }
}
