using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeRecoveryState : EnemyState
{
    private EnemyMelee enemy;
    public MeleeRecoveryState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = enemyBase as EnemyMelee;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.agent.isStopped = true;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        enemy.transform.rotation = enemy.FaceTarget(enemy.player.position);

        if(triggerCalled)
        {
            if(enemy.CanThrowAxe())
                stateMachine.ChangeState(enemy.abilityState);
            else if (enemy.PlayerInAttackRange())
                 stateMachine.ChangeState(enemy.attackState);
            else
                stateMachine.ChangeState(enemy.chaseState);
        }
    }
}
