using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, Control.IPlayerActions
{
    Vector2 _inputVector;
    [SerializeField]
    Animator _anim;
    [SerializeField]
    float _speed = 5f;


    // Start is called before the first frame update
    void Start()
    {

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
        //object pool blaster bolts
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
}
