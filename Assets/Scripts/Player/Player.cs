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
    Transform _blastOrigin;
    [SerializeField]
    float _blastForce;
    [SerializeField]
    float _blastCooldown = 0.15f;
    float _canBlast = -1f;
    [SerializeField]
    float _rapidfireRate = 0.125f;
    WaitForSeconds _rapidfireWait;
    public bool isDead;
    [SerializeField]
    List<Animator> _explosionAnimators;
    [SerializeField]
    AudioClip _explosionClip, _blasterClip;
    [SerializeField]
    AudioSource _audio;

    public float Speed => _speed;

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

        if (_inputVector.x > 0)
            _anim.SetBool("Accelerating", true);
        else if (_inputVector.x < 0)
            _anim.SetBool("Decelerating", true);
        else if (_inputVector.x == 0)
        {
            _anim.SetBool("Accelerating", false);
            _anim.SetBool("Decelerating", false);
        }
    }

    IEnumerator BlasterFireRoutine()
    {
        _canBlast = Time.time + _blastCooldown;
        for (int i = 0; i <= (int)_currentStrength; i++)
        {
            GameObject obj = PoolManager.Instance.RequestPoolObject(PoolManager.Instance.blastPool, PoolManager.Instance.blastPrefab, PoolManager.Instance.blastContainer);
            obj.transform.position = _blastOrigin.position;
            var velocity = _blastOrigin.forward * _blastForce;
            obj.GetComponent<Blast>().FireBlast(velocity);
            _audio.clip = _blasterClip;
            _audio.Play(); 
            yield return _rapidfireWait;
        }
    }

    public void Damage()
    {
        _currentStrength--;
        Debug.Log("Hit taken, current strength is " + _currentStrength);
        if (Health < 0)
        {
            foreach (var anim in _explosionAnimators)
            {
                anim.SetTrigger("Explode");
            }
            _audio.clip = _explosionClip;
            _audio.Play();
            isDead = true;
            Destroy(this.gameObject, 0.33f);
        }
    }

    public void PowerUp()
    {
        _anim.SetTrigger("Upgraded");
        _currentStrength++;
        if ((int)_currentStrength > 3)
            _currentStrength = WeaponStrength.Intermediate;
        Debug.Log("Powered up to " + _currentStrength + " strength!");
    }
}
