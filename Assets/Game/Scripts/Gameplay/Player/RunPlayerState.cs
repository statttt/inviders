using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunPlayerState : PlayerState
{
    public RunPlayerState(Player player, PlayerAnimator animator) : base(player, animator) { }

    public override void Enter()
    {
        _animator.SetState(AnimationsNames.Run, 0f);
        _player.PlayerMovement.IsMove = true;
    }

    public override void Update()
    {
        base.Update();
        if (_player.EnemysList.Count > 0)
        {
            _animator.Animator.SetFloat("BodyUp", 0);
        }
        else
        {
            _animator.Animator.SetFloat("BodyUp", .5f);
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        _player.PlayerMovement.Move();
    }

    public override void Exit()
    {
        _player.PlayerAnimator.SetAnimatorSpeed(1f);
        _player.PlayerMovement.Stop();
    }
}
