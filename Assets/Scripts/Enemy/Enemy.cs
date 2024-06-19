using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyStateMachine stateMachine {  get; private set; }


    public EnemyState idleState { get; private set; }
    public EnemyState moveState { get; private set; }

    void Start()
    {
        stateMachine = new EnemyStateMachine();
        idleState = new EnemyState(this, stateMachine, "Idle");
        moveState = new EnemyState(this, stateMachine, "Move");

        stateMachine.Initialize(idleState);
    }


    void Update()
    {
        stateMachine.currentState.Update();
    }
}
