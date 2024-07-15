 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAbilityState : EnemyState
{
    private EnemyMelee enemy;
    private Vector3 movementDirection;
    private float moveSpeed;

    private const float MAX_MOVEMENT_DISTANCE = 20f;
    public MeleeAbilityState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemyBase as EnemyMelee;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.PullWeapon();
        moveSpeed = enemy.moveSpeed;
        movementDirection = enemy.transform.position + (enemy.transform.forward * MAX_MOVEMENT_DISTANCE);
    }

    public override void Update()
    {
        base.Update();

        if (enemy.ManualRotationActive())
        {
            enemy.transform.rotation = enemy.FaceTarget(enemy.player.position);
            movementDirection = enemy.transform.position + (enemy.transform.forward * MAX_MOVEMENT_DISTANCE);
        }


        if (enemy.ManualMovementActive())
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, movementDirection, enemy.moveSpeed * Time.deltaTime);


        if (triggerCalled)
        {
            stateMachine.ChangeState(enemy.recoveryState);
        }
    }

    public override void AbilityTrigger()
    {
        base.AbilityTrigger();


        GameObject newAxe = ObjectPool.instance.GetObject(enemy.axePrefab);

        newAxe.transform.position = enemy.axeStartPoint.position;
        newAxe.GetComponent<EnemyAxe>().AxeSetup(enemy.axeFlySpeed, enemy.player, enemy.axeAimTimer);
    }

    public override void Exit()
    {
        base.Exit();
        enemy.moveSpeed = moveSpeed;
        enemy.anim.SetFloat("RecoveryIndex", 0);
    }
}
