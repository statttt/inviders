using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    public EnemyState CurrentState { get; set; }

    public void Initialize(EnemyState state)
    {
        CurrentState = state;
        CurrentState.Enter();
    }

    public void ChangeState(EnemyState newState)
    {
        if(newState.GetType() == CurrentState.GetType())
        {
            return;
        }
        if(CurrentState != null)
        {
            CurrentState.Exit();
        }
        CurrentState = newState;
        CurrentState.Enter();
    }
}
