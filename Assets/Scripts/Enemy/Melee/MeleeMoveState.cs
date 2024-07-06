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

    private Vector3 GetNextPathPoint()
    {
        NavMeshAgent agent = enemy.agent;
        NavMeshPath path = agent.path;

        if(path.corners.Length < 2)
            return agent.destination;

        for(int i = 0; i < path.corners.Length; i++)
        {
            if(Vector3.Distance(agent.transform.position, path.corners[i]) < 1)
                return path.corners[i + 1];

        }

        return agent.destination;
        
    }
}
