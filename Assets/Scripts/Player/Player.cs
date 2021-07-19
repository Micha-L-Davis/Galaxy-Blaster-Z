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
    AudioClip _explosionClip, _blasterClip, _powerupClip;
    [SerializeField]
    AudioSource _audio;
    [SerializeField]
    int _score;
    [SerializeField]
    List<GameObject> _megaBlastPrefabs, _waveBlastPrefabs;
    [SerializeField]
    GameObject _blackHolePrefab;
    [SerializeField]
    List<Transform> _waveOrigins, _blackHoleOrigins;
    GameObject _target;
    [SerializeField]
    Collider _collider;
    [SerializeField]
    CameraShake _camera;


    public float Speed => _speed;

    enum WeaponStrength
    {
        Basic,
        Intermediate,
        Advanced,
        SuperBasic,
        SuperIntermediate,
        SuperAdvanced
    };
    [SerializeField]
    WeaponStrength _currentStrength = WeaponStrength.Basic;

    enum Weapon
    {
        Blaster,
        MegaBlast,
        BlastWave,
        //BlackHole
    };
    [SerializeField]
    Weapon _currentWeapon = Weapon.Blaster;

    public int Health => (int)_currentStrength;


    // Start is called before the first frame update
    void Start()
    {
        _rapidfireWait = new WaitForSeconds(_rapidfireRate);
        Enemy.OnEnemyDeath += AddScore;
        UIManager.Instance.StrengthUpdate((int)_currentStrength);
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
        if (context.ReadValueAsButton() && Time.time > _canBlast)
        {
            switch (_currentWeapon)
            {
                case Weapon.Blaster:
                        StartCoroutine(BlasterFireRoutine());
                    break;
                case Weapon.MegaBlast:
                        FireMegaBlast();
                    break;
                case Weapon.BlastWave:
                        FireBlastWave();
                    break;
                //case Weapon.BlackHole:
                //        FireBlackHoleCannon();
                //    break;
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

    void FireMegaBlast()
    {
        double n = (int)_currentStrength / 2;
        _canBlast = Time.time + _blastCooldown;
        GameObject obj = Instantiate(_megaBlastPrefabs[(int)Math.Floor(n)], _blastOrigin.transform.position, Quaternion.identity);
        obj.transform.position = _blastOrigin.position;
        var velocity = _blastOrigin.forward * _blastForce;
        obj.GetComponent<Blast>().FireBlast(velocity);
        _audio.clip = _blasterClip;
        _audio.Play();

    }

    void FireBlastWave()
    {
        _canBlast = Time.time + _blastCooldown;
        GameObject obj;
        Vector3 velocity;
        switch (_currentStrength)
        {
            case WeaponStrength.SuperBasic:
                obj = Instantiate(_waveBlastPrefabs[0], _waveOrigins[0].position, Quaternion.identity);
                obj.transform.position = _waveOrigins[0].position;
                velocity = _waveOrigins[0].forward * _blastForce;
                obj.GetComponent<Blast>().FireBlast(velocity);
                _audio.clip = _blasterClip;
                _audio.Play();
                obj = Instantiate(_waveBlastPrefabs[1], _waveOrigins[1].position, Quaternion.identity);
                obj.transform.position = _waveOrigins[1].position;
                velocity = _waveOrigins[1].forward * _blastForce;
                obj.GetComponent<Blast>().FireBlast(velocity);
                _audio.clip = _blasterClip;
                _audio.Play();
                break;
            case WeaponStrength.SuperIntermediate:
                foreach (var origin in _waveOrigins)
                {
                    int i = 0;
                    obj = Instantiate(_waveBlastPrefabs[i], origin.position, Quaternion.identity);
                    obj.transform.position = origin.position;
                    velocity = origin.forward * _blastForce;
                    obj.GetComponent<Blast>().FireBlast(velocity);
                    _audio.clip = _blasterClip;
                    _audio.Play();
                    i++;
                }
                break;
            case WeaponStrength.SuperAdvanced:
                foreach (var origin in _waveOrigins)
                {
                    int i = 0;
                    obj = Instantiate(_waveBlastPrefabs[i], origin.position, Quaternion.identity);
                    obj.transform.position = origin.position;
                    velocity = origin.forward * _blastForce;
                    obj.GetComponent<Blast>().FireBlast(velocity);
                    _audio.clip = _blasterClip;
                    _audio.Play();
                    i++;
                }
                break;
            default:
                break;
        }
    }

    //void FireBlackHoleCannon()
    //{
    //    _target = ClosestEnemy();

    //    foreach (var item in _blackHoleOrigins)
    //    {
    //        var hole = Instantiate(_blackHolePrefab, item.position, Quaternion.identity);
    //        hole.transform.position = item.position;
    //        hole.GetComponent<BlackHole>().SetTarget(_target, (int)_currentStrength);
    //        _audio.clip = _blasterClip;
    //        _audio.Play();
    //    }
    //}

    //private GameObject ClosestEnemy()
    //{
    //    List<GameObject> enemies = SpawnManager.Instance.waves[SpawnManager.Instance.currentWave].spawned;
    //    float minDistance = Mathf.Infinity;
    //    foreach (var e in enemies)
    //    {
    //        float distance = Vector3.Distance(e.transform.position, transform.position);
    //        if (distance < minDistance)
    //        {
    //            _target = e;
    //            minDistance = distance;
    //        }
    //    }

    //    return _target;
    //}

    public void Damage()
    {
        _camera.ShakeCamera(.05f);
        if (Health == 4)
        {
            _currentWeapon = Weapon.Blaster;
        }       
        _currentStrength--;
        StartCoroutine(DisableColliderRoutine());
        UIManager.Instance.StrengthUpdate((int)_currentStrength);
        Debug.Log("Hit taken, current strength is " + _currentStrength);
        if (Health < 0)
        {
            foreach (var anim in _explosionAnimators)
            {
                anim.SetTrigger("Explode");
            }
            _audio.clip = _explosionClip;
            OnPlayerDeath?.Invoke();
            _audio.Play();
            isDead = true;
            Destroy(this.gameObject, 0.4f);
        }
    }

    IEnumerator DisableColliderRoutine()
    {
        _collider.enabled = false;
        yield return new WaitForSeconds(.5f);
        _collider.enabled = true;
        
    }

    public void PowerUp(int weaponType)
    {
        _anim.SetTrigger("Upgraded");
        _audio.clip = _powerupClip;
        _audio.Play();
        _currentStrength++;
        if ((int)_currentStrength > 3)
        {
            switch (weaponType)
            {
                case 0:
                    _currentStrength++;
                    break;
                case 1:
                    _currentWeapon = Weapon.MegaBlast;
                    break;
                case 2:
                    _currentWeapon = Weapon.BlastWave;
                    break;
                //case 3:
                //    _currentWeapon = Weapon.BlackHole;
                //    break;
                default:
                    break;
            }
        }
        UIManager.Instance.StrengthUpdate(Health);
        Debug.Log("Powered up to " + Health + " strength!");
    }

    void AddScore(GameObject enemy)
    {
        _score += enemy.GetComponent<Enemy>().scoreValue;
        UIManager.Instance.ScoreUpdate(_score);
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        UIManager.Instance.Pause();
    }
}
