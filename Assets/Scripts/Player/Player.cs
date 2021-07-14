using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, Control.IPlayerActions, IDamageable
{
    public static Action OnPlayerDeath;

    Vector2 _inputVector;
    [SerializeField]
    Animator _anim;
    [SerializeField]
    float _speed = 5f;
    [SerializeField]
    PoolManager _poolManager;
    [SerializeField]
    Transform _blastOrigin;
    [SerializeField]
    float _blastCooldown = 0.15f;
    float _canBlast = -1f;
    [SerializeField]
    float _rapidfireRate = 0.125f;
    WaitForSeconds _rapidfireWait;


    enum WeaponStrength
    {
        Basic,
        Intermediate,
        Advanced
    };
    [SerializeField]
    WeaponStrength _currentStrength = WeaponStrength.Basic;

    enum Weapon
    {
        Blaster,
        SplitFire
    };
    [SerializeField]
    Weapon _currentWeapon = Weapon.Blaster;

    public int Health => (int)_currentStrength;


    // Start is called before the first frame update
    void Start()
    {
        _rapidfireWait = new WaitForSeconds(_rapidfireRate);
    }

    // Update is called once per frame
    void Update()
    {
        var velocity = _inputVector * _speed * Time.deltaTime;
        var x = Mathf.Clamp((transform.position.x + velocity.x), -28, 29);
        var y = Mathf.Clamp((transform.position.y + velocity.y), -17, 17);
        
        transform.position = new Vector3(x, y, 0);
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton())
        {
            switch (_currentWeapon)
            {
                case Weapon.Blaster:
                    if (Time.time > _canBlast)
                    {
                        StartCoroutine(BlasterFireRoutine());
                    }

                    break;
                default:
                    break;
            }

        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _inputVector = context.ReadValue<Vector2>();
        if (_inputVector.y > 0)
            _anim.SetBool("Climbing", true);
        else if (_inputVector.y < 0)
            _anim.SetBool("Diving", true);
        else if (_inputVector.y == 0)
        {
            _anim.SetBool("Climbing", false);
            _anim.SetBool("Diving", false);
        }
    }

    IEnumerator BlasterFireRoutine()
    {
        _canBlast = Time.time + _blastCooldown;
        for (int i = 0; i <= (int)_currentStrength; i++)
        {
            GameObject obj = _poolManager.RequestPoolObject(_poolManager._blastPool);
            obj.transform.position = _blastOrigin.position;
            yield return _rapidfireWait;
        }
    }

    public void Damage()
    {
        _currentStrength--;
        if (Health < 0)
            Destroy(this, 1f);
    }
}
