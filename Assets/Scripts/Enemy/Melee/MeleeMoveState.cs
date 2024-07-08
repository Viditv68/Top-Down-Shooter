using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
        enemy.agent.speed = enemy.moveSpeed;
        destination = enemy.GetPatrolDestination();
        enemy.agent.SetDestination(destination);
    }

    public override void Exit()
    {
        base.Exit();

    }

    public override void Update()
    {
        base.Update();

        if (enemy.PlayerInAgressionRange())
        {
            stateMachine.ChangeState(enemy.recoveryState);
            return;
        }
           

        enemy.transform.rotation = enemy.FaceTarget(GetNextPathPoint());

        if(enemy.agent.remainingDistance <= enemy.agent.stoppingDistance + 0.5f)
        {
            stateMachine.ChangeState(enemy.idleState);
            Debug.Log("Changed to idle state " 
                + stateTimer);
        }
    }

}
