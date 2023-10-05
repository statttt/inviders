using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class PlayerMovement : MonoBehaviour
{
    private Player _player;


    [SerializeField] private float _startSpeed;
    [SerializeField] private float _currentSpeed;
    [SerializeField] private float _speedRotate;
    [SerializeField] private float _speedRotateMove;

    private Quaternion lastRotation;

    private NavMeshAgent _agent;
    private Joystick _joystick;

    public bool IsMove { get; set; }

    public bool IsMoving;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        _joystick = UIControls.Instance.Joystick;
        _player = Player.Instance;
    }

    public void UpdateSpeed()
    {
        float ratio = (float)Math.Pow(UIUpgrade.Instance.ratioSpeedPlayer, UIUpgrade.Instance.LevelPlayerSpeed);
        _currentSpeed = _startSpeed * ratio;
    }

    private void Update()
    {
        if (_player.IsDie)
        {
            return;
        }
        if(_joystick.Direction.magnitude > 0)
        {
            if (Input.GetMouseButton(0) && !IsMove)
            {
                _player.StateMachine.ChangeState(new RunPlayerState(_player, _player.PlayerAnimator));
            }
        }
        else
        {
            if (Input.GetMouseButtonUp(0))
            {
                _player.StateMachine.ChangeState(new IdlePlayerState(_player, _player.PlayerAnimator));
            }
        }
    }

    public void Move()
    {
        IsMoving = false;
        if (Math.Abs(_joystick.Horizontal) > 0.05f || Math.Abs(_joystick.Vertical) > 0.05f)
        {
            IsMoving = true;
            Vector3 inputVector = (Vector3.forward * _joystick.Vertical) + (Vector3.right * _joystick.Horizontal);

            inputVector = Quaternion.AngleAxis(-45f, Vector3.up) * inputVector;

            _agent.velocity = new Vector3(inputVector.x * _currentSpeed, _agent.velocity.y, inputVector.z * _currentSpeed);

            if (!_player.PlayerShooting.Target)
            {
                //_player.TargetForRotate.position = transform.position + inputVector.normalized * 10f;
                RotateToTargetMove(transform.position + inputVector.normalized * 10f);
            }
            else
            {
                RotateToTarget(_player.PlayerShooting.Target.transform.position);
            }

            _player.PlayerAnimator.MoveIK(inputVector.magnitude);

            Vector3 animationVector = transform.InverseTransformDirection(inputVector);

            var VelocityX = animationVector.x;
            var VelocityZ = animationVector.z;
            _player.PlayerAnimator.Animator.SetFloat("VelocityX", VelocityX);
            _player.PlayerAnimator.Animator.SetFloat("VelocityZ", VelocityZ);

        }
        else
        {
            _player.PlayerAnimator.Animator.SetFloat("VelocityX", 0);
            _player.PlayerAnimator.Animator.SetFloat("VelocityZ", 0);
        }
    }

    public void RotateToTarget(Vector3 targetPoint)
    {
        _player.TargetForRotate.position = Vector3.Lerp(_player.TargetForRotate.position, targetPoint, _speedRotate);
    }

    public void RotateToTargetMove(Vector3 targetPoint)
    {
        _player.TargetForRotate.position = Vector3.Lerp(_player.TargetForRotate.position, targetPoint, _speedRotateMove);
    }

    public void Stop()
    {
        _agent.ResetPath();
        _agent.velocity = Vector3.zero;
        IsMove = false;
        IsMoving = false;
    }

    public void Deactivate()
    {
        _agent.enabled = false;
    }

    //public void RotateToTarget()
    //{
    //    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(_player.PlayerShooting.Target.transform.position - transform.position), _speedRotate);
    //}
}
