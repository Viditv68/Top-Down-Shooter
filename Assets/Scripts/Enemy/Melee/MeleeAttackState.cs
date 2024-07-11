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
        attackMoveSpeed = enemy.attackData.moveSpeed;
        enemy.anim.SetFloat("AttackAnimationSpeed", enemy.attackData.animationSpeed);
        enemy.anim.SetFloat("AttackIndex", enemy.attackData.attackIndex);
        
        enemy.agent.isStopped = true;
        enemy.agent.velocity = Vector3.zero;



        attackDirection = enemy.transform.position + (enemy.transform.forward * MAX_ATTACK_DISTANCE);
    }

    public override void Exit()
    {
        base.Exit();
        SetupNextAttack();
    }

    private void SetupNextAttack()
    {
        int recoveryIndex = PlayerClose() ? 1 : 0;
        enemy.anim.SetFloat("RecoverIndex", recoveryIndex);

        enemy.attackData = UpdatedAttackData();
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

    private AttackData UpdatedAttackData()
    {
        List<AttackData> validAttacks = new List<AttackData>(enemy.attackList);


        if(PlayerClose()) 
        {
            validAttacks.RemoveAll(x => x.attackType == AttackTypeMelee.Charge);
        }

        int random = Random.Range(0, validAttacks.Count);
        return validAttacks[random];
    }


    private bool PlayerClose() => Vector3.Distance(enemy.transform.position, enemy.player.position) <= 1;

}
