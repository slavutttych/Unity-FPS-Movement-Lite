using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speedWalk;
    [SerializeField] private float _gravity;
    [SerializeField] private float _jumpPower;
    [SerializeField] private float _speedRun;
    [SerializeField] private float _speedCrouch;
    [SerializeField] private float _speedDash;

    [SerializeField] private float _timeDash;
    [SerializeField] private float _cooldownDash;

    private CharacterController _characterController;
    private Vector3 _walkDirection;
    private Vector3 _velocity;
    private float _speed;
    private float _cdTimerDash;

    private void Start()
    {
        _speed = _speedWalk;
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        // Inputs
        Jump(Input.GetKey(KeyCode.Space) && _characterController.isGrounded);
        Run(Input.GetKey(KeyCode.LeftShift));
        Sit(Input.GetKey(KeyCode.LeftControl));
        
        // Dash Cooldown Timer
        if (Time.time > _cdTimerDash)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                StartCoroutine(Dash());
                _cdTimerDash = Time.time + _cooldownDash;
            }
        }
       
        // Directions Input
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Movement Direction
        _walkDirection = (transform.right * x + transform.forward * z);
    }

    private void FixedUpdate()
    {
        Walk(_walkDirection);
        DoGravity(_characterController.isGrounded);
    }

    private void Walk(Vector3 direction)
    {
        _characterController.Move((direction * _speedWalk) * Time.fixedDeltaTime);
    }

    private void DoGravity(bool isGrounded)
    {
        if (isGrounded && _velocity.y < 0)
            _velocity.y = -1f;
        _velocity.y -= _gravity * Time.fixedDeltaTime;
        _characterController.Move(_velocity * Time.fixedDeltaTime);
    }

    private void Jump(bool canJump)
    {
        if (canJump)
            _velocity.y = _jumpPower;
    }

    private void Run(bool canRun)
    {
        _speedWalk = canRun ? _speedRun : _speed;
    }

    private void Sit(bool canSit)
    {
        _characterController.height = canSit ? 1f : 2f;
        if (canSit)
            _speedWalk = canSit ? _speedCrouch : _speed;
    }

    private IEnumerator Dash()
    {
        float _timeStart = Time.time;

         while (Time.time < _timeStart + _timeDash)
         {
              _characterController.Move((_walkDirection * _speedDash) * Time.fixedDeltaTime);

              yield return null;
         }
    }
}