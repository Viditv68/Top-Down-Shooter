using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine
{
    public EnemyState currentState {  get; private set; }

    public void Initialize(EnemyState _startState)
    {
        currentState = _startState;

        currentState.Enter();
    }

    public void ChangeState(EnemyState _newState)
    {
        Debug.Log("previous state: " + currentState);
        currentState.Exit();
        currentState = _newState;
        Debug.Log("Current state: " + currentState);
        currentState.Enter();
    }
}
