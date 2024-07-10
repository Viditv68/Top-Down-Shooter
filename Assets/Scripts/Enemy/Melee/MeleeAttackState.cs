using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackState : EnemyState
{
    private EnemyMelee enemy;
    private Vector3 attackDirection;
    private float attackMoveSpeed;
    private const float MAX_ATTACK_DISTANCE = 50f;
    public MeleeAttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemyBase as EnemyMelee;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.PullWeapon();
        enemy.agent.isStopped = true;
        enemy.agent.velocity = Vector3.zero;

        attackMoveSpeed = enemy.attackData.moveSpeed;

        enemy.anim.SetFloat("AttackAnimationSpeed", enemy.attackData.animationSpeed);
        enemy.anim.SetFloat("AttackIndex", enemy.attackData.attackIndex);

        attackDirection = enemy.transform.position + (enemy.transform.forward * MAX_ATTACK_DISTANCE);
    }

    public override void Exit()
    {
        base.Exit();

        enemy.anim.SetFloat("RecoveryIndex", 0);

        if (enemy.PlayerInAttackRange())
            enemy.anim.SetFloat("RecoveryIndex", 1);
    }

    public override void Update()
    {
        base.Update();

        if (enemy.ManualRotationActive())
        {
            enemy.transform.rotation = enemy.FaceTarget(enemy.player.position);
            attackDirection = enemy.transform.position + (enemy.transform.forward * MAX_ATTACK_DISTANCE);
        }


        if(enemy.ManualMovementActive())
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, attackDirection, attackMoveSpeed * Time.deltaTime);


        if (triggerCalled)
        {
            if(enemy.PlayerInAttackRange())
                stateMachine.ChangeState(enemy.recoveryState);
            else
                stateMachine.ChangeState(enemy.chaseState);

        }
            
    }
}
