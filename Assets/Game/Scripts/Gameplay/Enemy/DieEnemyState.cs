using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieEnemyState : EnemyState
{
    public DieEnemyState(Enemy enemy, EnemyAnimator animator) : base(enemy, animator) { }

    public override void Enter()
    {
        _enemy.CoinsGroup.transform.parent = Level.Instance.transform;
        _enemy.CoinsGroup.Activate(_enemy.Level);
        _enemy.IsDie = true;
        if (_enemy.IsDestroyer)
        {
            _enemy.Zone.RemoveAttackEnemy(_enemy);
        }
        else
        {
            _enemy.Zone.RemoveEnemy(_enemy);
        }
        _enemy.EnemyMovement.DeactivateAgent();
        _animator.DeactivateAnimator();
        _enemy.EnemyRagdoll.ActivateRagdoll();
        Player.Instance.RemoveEnemy(_enemy);
        _enemy.Collider.enabled = false;
        _enemy.RemoveAllAllies();
    }

    public override void Exit()
    {

    }
}
