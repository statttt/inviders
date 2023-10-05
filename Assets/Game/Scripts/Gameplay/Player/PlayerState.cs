using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public abstract class PlayerState
{
    protected Player _player;
    protected PlayerAnimator _animator;

    protected PlayerState(Player player,PlayerAnimator animator)
    {
        _player = player;
        _animator = animator;
    }

    public abstract void Enter();

    public abstract void Exit();

    public virtual void Update()
    {

    }

    public virtual void FixedUpdate()
    {

    }
}
