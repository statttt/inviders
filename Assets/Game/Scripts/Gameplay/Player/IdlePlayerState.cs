using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdlePlayerState : PlayerState
{
    public IdlePlayerState(Player player, PlayerAnimator animator) : base(player, animator) { }

    public override void Enter()
    {
        _animator.SetState(AnimationsNames.Idle, 0.1f);
        _player.PlayerMovement.Stop();
    }

    public override void Update()
    {
        base.Update();
        _animator.Animator.SetFloat("BodyUp", 0);
    }

    public override void Exit()
    {
        
    }

}
