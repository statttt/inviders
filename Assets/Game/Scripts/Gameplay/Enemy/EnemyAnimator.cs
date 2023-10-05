using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetState(string animationName, float time)
    {
        if(_animator != null )
        {
            _animator.CrossFade(animationName, time);
        }
    }


    public void DeactivateAnimator()
    {
        if(_animator != null)
        {
            _animator.enabled = false;
        }
    }

    public void Attack()
    {
        _enemy.EnemyAttack.Attack();
    }


    public void TryAttack()
    {
        _enemy.EnemyAttack.TryPlayer();
    }
}
