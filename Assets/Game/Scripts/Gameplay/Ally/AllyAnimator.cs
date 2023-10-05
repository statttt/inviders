using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyAnimator : MonoBehaviour
{
    [SerializeField] private Ally _ally;
    [SerializeField] private Transform _rightTarget;
    [SerializeField] private Transform _leftTarget;
    [SerializeField] private Transform _idleRightHand;
    [SerializeField] private Transform _idleLeftHand;
    [SerializeField] private Transform _shootRightHand;
    [SerializeField] private Transform _shootLeftHand;

    private float _weightRightHand;
    private float _weightLefttHand;

    private Animator _animator;

    public bool isActiveIK;

    public bool isFear;

    public Animator Animator { get { return _animator; } }


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

    private void LateUpdate()
    {
        if (_ally.IsFear)
        {
            Fear();
        }
        else
        {
            IdleIK();
        }
    }

    private void OnAnimatorIK()
    {
        if (isActiveIK)
        {
            _weightRightHand = 1;
            _weightLefttHand = 1;

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
        if (!_ally.AllyShooting.Target)
        {
            SetHandsPoints(_idleRightHand, _idleLeftHand);
        }
        else
        {
            SetHandsPoints(_shootRightHand, _shootLeftHand);
        }
    }

    public void Fear()
    {
        isActiveIK = false;
    }

    public void SetIsFear(bool isFear)
    {
        _animator.SetBool("IsFear", isFear);
    }
}
