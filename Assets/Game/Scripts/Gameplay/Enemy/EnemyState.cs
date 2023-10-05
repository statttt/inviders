using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyState
{
    protected Enemy _enemy;
    protected EnemyAnimator _animator;

    protected EnemyState(Enemy enemy, EnemyAnimator animator)
    {
        _enemy = enemy;
        _animator = animator;
    }

    public abstract void Enter();

    public abstract void Exit();

    public virtual void Update()
    {

    }

    public virtual void LateUpdate()
    {

    }
}
