using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    float _speed = 4;

    enum Weapon
    {
        Blaster,
        MegaBlast,
        BlastWave,
        //BlackHole
    };
    [SerializeField]
    Weapon _weaponType;

    private void Update()
    {
        transform.Translate(Vector3.left * _speed * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Powerup obtained.");
        Player player = other.GetComponent<Player>();
        player.PowerUp((int)_weaponType);
        Destroy(this.gameObject);
    }
}
