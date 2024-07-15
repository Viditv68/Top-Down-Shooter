using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeIdleState : EnemyState
{
    private EnemyMelee enemy;
    public MeleeIdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemyBase as EnemyMelee;
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = enemyBase.idleTime;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
        {
            Debug.Log("Changed to move state " + stateTimer);
            stateMachine.ChangeState(enemy.moveState);
        }

    }
}
