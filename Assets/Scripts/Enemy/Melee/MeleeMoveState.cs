using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeMoveState : EnemyState
{
    private EnemyMelee enemy;
    private Vector3 destination;
    public MeleeMoveState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = enemyBase as EnemyMelee;
    }

    public override void Enter()
    {
        base.Enter();
        destination = enemy.GetPatrolDestination();
    }

    public override void Exit()
    {
        base.Exit();

    }

    public override void Update()
    {
        base.Update();

        enemy.agent.SetDestination(destination);
        if(enemy.agent.remainingDistance <= 1 )
        {
            stateMachine.ChangeState(enemy.idleState);
            Debug.Log("Changed to idle state " 
                + stateTimer);
        }
    }
}
