using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Transform _rightTarget;
    [SerializeField] private Transform _leftTarget;
    [SerializeField] private Transform _idleRightHand;
    [SerializeField] private Transform _idleLeftHand;
    [SerializeField] private Transform _moveRightHand;
    [SerializeField] private Transform _moveLeftHand;

    private Vector3 _offsetPosRightHand;
    private Vector3 _offsetPosLeftHand;
    private Vector3 _offsetRotRightHand;
    private Vector3 _offsetRotLeftHand;

    private float _weightRightHand;
    private float _weightLefttHand;

    private Animator _animator;

    public bool isActiveIK;

    public Animator Animator { get { return _animator; } }

    private void Start()
    {
        _offsetPosRightHand = _moveRightHand.localPosition - _idleRightHand.localPosition;
        _offsetPosLeftHand = _moveLeftHand.localPosition - _idleLeftHand.localPosition;
        _offsetRotRightHand = _moveRightHand.eulerAngles - _idleRightHand.eulerAngles;
        _offsetRotLeftHand = _moveLeftHand.eulerAngles - _idleLeftHand.eulerAngles;
        if(_offsetRotRightHand.x > 180)
        {
            _offsetRotRightHand -= new Vector3(360, 0, 0);
        }
        if(_offsetRotRightHand.x < -180)
        {
            _offsetRotRightHand += new Vector3(360, 0, 0);
        }
        if(_offsetRotRightHand.y > 180)
        {
            _offsetRotRightHand -= new Vector3(0, 360, 0);
        }
        if(_offsetRotRightHand.y < -180)
        {
            _offsetRotRightHand += new Vector3(0, 360, 0);
        }
        if(_offsetRotRightHand.z > 180)
        {
            _offsetRotRightHand -= new Vector3(0, 0, 360);
        }
        if(_offsetRotRightHand.z < -180)
        {
            _offsetRotRightHand += new Vector3(0, 0, 360);
        }
        if(_offsetRotLeftHand.x > 180)
        {
            _offsetRotLeftHand -= new Vector3(360, 0, 0);
        }
        if(_offsetRotLeftHand.x < -180)
        {
            _offsetRotLeftHand += new Vector3(360, 0, 0);
        }
        if(_offsetRotLeftHand.y > 180)
        {
            _offsetRotLeftHand -= new Vector3(0, 360, 0);
        }
        if(_offsetRotLeftHand.y < -180)
        {
            _offsetRotLeftHand += new Vector3(0, 360, 0);
        }
        if(_offsetRotLeftHand.z > 180)
        {
            _offsetRotLeftHand -= new Vector3(0, 0, 360);
        }
        if(_offsetRotLeftHand.z < -180)
        {
            _offsetRotLeftHand += new Vector3(0, 0, 360);
        }
    }

    public void SetHandsPoints(Transform rightHandPoint, Transform leftHandPoint)
    {
        _rightTarget.localPosition = rightHandPoint.localPosition;
        _rightTarget.localEulerAngles = rightHandPoint.localEulerAngles;
        _leftTarget.localPosition = leftHandPoint.localPosition;
        _leftTarget.localEulerAngles = leftHandPoint.localEulerAngles;
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetState(string animationName,float time)
    {
        _animator.CrossFade(animationName, time);
    }

    public void SetAnimatorSpeed(float speed)
    {
        _animator.speed = speed;
    }


    private void OnAnimatorIK()
    {
        if (isActiveIK)
        {
            Animator.SetIKPositionWeight(AvatarIKGoal.RightHand, _weightRightHand);

            Animator.SetIKRotationWeight(AvatarIKGoal.RightHand, _weightRightHand);

            Animator.SetIKPosition(AvatarIKGoal.RightHand, _rightTarget.position);
            Animator.SetIKRotation(AvatarIKGoal.RightHand, _rightTarget.rotation);

            Animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, _weightLefttHand);

            Animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, _weightLefttHand);

            Animator.SetIKPosition(AvatarIKGoal.LeftHand, _leftTarget.position);
            Animator.SetIKRotation(AvatarIKGoal.LeftHand, _leftTarget.rotation);
        }

    }

    public void IdleIK()
    {
        isActiveIK = true;
        _weightLefttHand = 1;
        _weightRightHand = 1;
        if (!Player.Instance.PlayerShooting.Target)
        {
            SetHandsPoints(_idleRightHand, _idleLeftHand);
        }
        else
        {
            SetHandsPoints(Player.Instance.PlayerShooting.CurrentWeapon.RightTarget, Player.Instance.PlayerShooting.CurrentWeapon.LeftTarget);
        }
    }

    public void MoveIK(float offset)
    {
        isActiveIK = true;
        if (Player.Instance.PlayerShooting.Target)
        {
            _weightLefttHand = 1;
            _weightRightHand = 1;
            SetHandsPoints(Player.Instance.PlayerShooting.CurrentWeapon.RightTarget, Player.Instance.PlayerShooting.CurrentWeapon.LeftTarget);
        }
        else
        {
            _weightLefttHand = 1f - offset;
            _weightRightHand = 1;
            _rightTarget.localPosition = _idleRightHand.localPosition + _offsetPosRightHand * offset;
            _rightTarget.eulerAngles = _idleRightHand.eulerAngles + _offsetRotRightHand * offset;
            _leftTarget.localPosition = _idleLeftHand.localPosition + _offsetPosLeftHand * offset;
            _leftTarget.eulerAngles = _idleLeftHand.eulerAngles + _offsetRotLeftHand * offset;
        }
    }

}
