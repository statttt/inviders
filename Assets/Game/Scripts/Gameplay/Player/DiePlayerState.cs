using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiePlayerState : PlayerState
{
    public DiePlayerState(Player player, PlayerAnimator animator) : base(player, animator) { }

    public override void Enter()
    {
        _player.IsDie = true;
        _animator.SetState(AnimationsNames.Die, 0f);
        _player.PlayerHealth.Deactivate();
        _player.PlayerMovement.Deactivate();
        _animator.Animator.SetLayerWeight(1, 0);
        UIManager.Instance.ChangeState(UIState.Lose);
    }

    public override void Exit()
    {

    }

}
