using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyState
{
    public EnemyAttackState(Enemy enemy, EnemyAnimator animator) : base(enemy, animator) { }

    public override void Enter()
    {
        _animator.SetState(AnimationsNames.Attack, 0f);
    }

    public override void Exit()
    {
        
    }

    public override void LateUpdate()
    {
        base.LateUpdate();
        _enemy.EnemyAttack.LookAtTarget();
    }

}
