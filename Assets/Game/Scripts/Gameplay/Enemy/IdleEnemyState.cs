using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleEnemyState : EnemyState
{
    public IdleEnemyState(Enemy enemy, EnemyAnimator animator) : base(enemy, animator) { }

    public override void Enter()
    {
        _animator.SetState(AnimationsNames.Idle, 0.1f);
    }

    public override void Exit()
    {

    }
}
