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
    bool _concealed;

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
            Debug.Log("Enemy Damage Avoided Due To Concealment");
            return;
        }
        Debug.Log("Enemy Taking Damage");
        _health--;
        if (Health < 1)
        {
            if (OnEnemyDeath != null)
            {
                Debug.Log("Sending Death Notice");
                OnEnemyDeath(this.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    protected void OnTriggerEnter(Collider other)
    {
        IDamageable i = other.GetComponent<IDamageable>();
        if (i != null && other.tag != "Enemy")
        {
            i.Damage();
        }
    }
}
