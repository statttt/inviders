using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class RunEnemyState : EnemyState
{
    public RunEnemyState(Enemy enemy, EnemyAnimator animator) : base(enemy, animator) { }

    public override void Enter()
    {
        _animator.SetState(AnimationsNames.Run, 0f);
        _enemy.EnemyMovement.IsMove = true;
    }

    public override void Update()
    {
        base.Update();
        _enemy.EnemyMovement.Move();
    }

    public override void LateUpdate()
    {
        base.LateUpdate();
        _enemy.EnemyMovement.LookAtTarget();
    }

    public override void Exit()
    {
        _enemy.EnemyMovement.IsMove = false;
        _enemy.EnemyMovement.Stop();
    }
}
