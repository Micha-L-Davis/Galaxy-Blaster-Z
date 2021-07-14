using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable, IConcealable
{

    protected int _health;
    bool _concealed;

    public int Health { get; }

    public void ToggleConcealment()
    {
        if (!_concealed)
            _concealed = true;
        else
            _concealed = false;
        Debug.Log("Concealed = " + _concealed);
    }

    public void Damage()
    {
        if (_concealed)
            return;
        _health--;
        if (Health < 0)
            Destroy(this, 1f);
    }
}
