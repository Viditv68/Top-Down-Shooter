using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeDeathState : EnemyState
{
    private EnemyMelee enemy;
    private EnemyRagdoll ragdoll;

    private bool interactionDisabled;
    public MeleeDeathState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = enemyBase as EnemyMelee;
        ragdoll = enemy.GetComponent<EnemyRagdoll>();
    }

    public override void Enter()
    {
        base.Enter();
        interactionDisabled = false;
        enemy.anim.enabled = false;
        enemy.agent.isStopped = true;

        ragdoll.RagdollActive(true);

        stateTimer = 1.5f;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0 && !interactionDisabled)
        {
            interactionDisabled = true;
            ragdoll.RagdollActive(false);
            ragdoll.CollidersActive(false);
        }

    }
}
